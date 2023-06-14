using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterStatesController : MonoBehaviour
{
	[FormerlySerializedAs("characterData")] public CharacterData CharacterData;
	
	private CharacterController characterController;
	[HideInInspector] public CollisionFlags collisionFlags;

	public MovementRecord MovementRecord;


	[HideInInspector] public ClampedFloatValue TimeMoving;

	public StaminaSystem StaminaSystem;

	[HideInInspector] public float TimeElapsed = 0f;
	
	public ECharacterState behaviourState;
	[HideInInspector] public EPhysicsState physicsState;
	[HideInInspector] public ECharacterSubstate substate;
	
	private IStateBehaviour walkingMovement;
	private IStateBehaviour sprintingMovement;
	private IStateBehaviour airbornMovement;
	private IStateBehaviour dodgeState;
	private IStateBehaviour hitstunState;
	[HideInInspector] public AttackingState attackState;

	private PhysicsState groundedPhysics;
	private PhysicsState airbornPhysics;
	
	private ISubstateBehaviour blockingSubstate;
	private ISubstateBehaviour nothingSubstate;
	
	public IStateBehaviour currentBehaviourState;
	public PhysicsState currentPhysicsState;
	public ISubstateBehaviour currentSubstate;

	[HideInInspector] public Animator Animator;

	public EquipmentData rightHandEquipment;
	public EquipmentData leftHandEquipment;
	
	public AttacksHurtboxController attacksHurtboxController;

	public ECharacterType CharacterType;
	public ECharacterType CanAttackType;
	
	[HideInInspector] public HealthSystem HealthSystem;

	[HideInInspector] public bool IsLocked;
	[HideInInspector] public Transform LockedTarget;
	[HideInInspector] public Transform LookAtObject;

	[HideInInspector] public bool TryingToRun;
	private AttackComboData nextAttackCombo;
	
	private void Awake()
	{
		Animator = GetComponentInChildren<Animator>();
		characterController = GetComponent<CharacterController>();
		TimeMoving = new ClampedFloatValue(0f, 0f, CharacterData.WalkingMovementData.Acceleration);
		
		HealthSystem = GetComponent<HealthSystem>();
		
		MovementRecord = new MovementRecord();
		StaminaSystem = new StaminaSystem(CharacterData);

		attacksHurtboxController = GetComponentInChildren<AttacksHurtboxController>();

		if (leftHandEquipment == null)
			leftHandEquipment = CharacterData.StartingLeftHandEquipment;
		
		if (rightHandEquipment == null)
			rightHandEquipment = CharacterData.StartingRightHandEquipment;
	}

	void Start()
	{
		//Behaviour states
		if(CharacterData.WalkingMovementData != null)
			walkingMovement = new GroundMovement(this, CharacterData.WalkingMovementData);
		if(CharacterData.SprintingMovementData != null)
			sprintingMovement = new GroundMovement(this, CharacterData.SprintingMovementData);
		if(CharacterData.AirbornMovementData != null)
			airbornMovement = new AirbornMovement(this);
		if(CharacterData.DodgeData != null)
			dodgeState = new DodgingState(this);
		if(CharacterData.StartingRightHandEquipment != null || rightHandEquipment != null)
			attackState = new AttackingState(this);
		if(CharacterData.HitstunData != null)
			hitstunState = new HitstunState(this);

		//Physics states
		groundedPhysics = new PhysicsGrounded(this);
		airbornPhysics = new PhysicsAirborn(this);
		
		//Substates
		blockingSubstate = new BlockingSubstate(this);
		nothingSubstate = new NothingSubstate();

		Initialize();
	}

	public void Initialize()
	{
		currentBehaviourState = walkingMovement;
		currentBehaviourState.OnEnter();
		behaviourState = ECharacterState.WALKING;
		
		currentPhysicsState = groundedPhysics;
		currentPhysicsState.OnEnter();
		physicsState = EPhysicsState.GROUNDED;
		
		currentSubstate = nothingSubstate;
		substate = ECharacterSubstate.NOTHING;
		
		//FindObjectOfType<RagdollActivator>().SetState(false);
	}
	
	private void Update()
	{
		if (behaviourState == ECharacterState.DEAD)
			return;
		
		var deltaTime = Time.deltaTime;
		TimeElapsed += deltaTime;

		StaminaSystem.Update(deltaTime);
		
		currentSubstate.Update(deltaTime);
		currentBehaviourState.Update(deltaTime);
		
		MovementRecord.Movement = AdjustVelocityToSlope(MovementRecord.Movement);

		//Physics state
		//forceApplier.Execute(deltaTime);
		currentPhysicsState.Update(deltaTime);

		//Move and update collisionFlags
		ApplyMovement(MovementRecord);
		
		if (TryingToRun && behaviourState != ECharacterState.SPRINTING)
		{
			ChangeState(ECharacterState.SPRINTING);
		}
		else if (!TryingToRun && behaviourState == ECharacterState.SPRINTING)
		{
			ChangeState(ECharacterState.WALKING);
		}
	}

	Vector3 AdjustVelocityToSlope(Vector3 movement)
	{
		var ray = new Ray(transform.position, Vector3.down);

		if (Physics.Raycast(ray, out RaycastHit hitInfo, 0.3f))
		{
			var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
			var adjusterMovement = slopeRotation * movement;

			if (behaviourState != ECharacterState.AIRBORN && adjusterMovement.y < 0)
			{
				return adjusterMovement;
			}
		}

		return movement;
	}

	private void ApplyMovement(MovementRecord movementRecord)
	{
		collisionFlags = characterController.Move(movementRecord.Movement);
	}

	public void SetAttackState(AttackComboData comboData, EWeaponSlot weaponSlot)
	{
		if (comboData == null || !CanChangeState(ECharacterState.ATTACKING))
			return;

		attacksHurtboxController.SetWeapon(weaponSlot);
		//Debug.Log("Change combo attack");
		nextAttackCombo = comboData;
		ChangeState(ECharacterState.ATTACKING);
	}
	
	public void ChangeState(ECharacterState newState)
	{
		IStateBehaviour newBehaviourState;
		
		if (!CanChangeState(newState) || behaviourState == ECharacterState.DEAD)
			return;
		
		switch (newState)
		{
			case ECharacterState.AIRBORN:
				newBehaviourState = airbornMovement;
				break;
			case ECharacterState.DODGING:
				newBehaviourState = dodgeState;
				break;
			case ECharacterState.WALKING:
				newBehaviourState = walkingMovement;
				break;
			case ECharacterState.ATTACKING:
				newBehaviourState = attackState;
				attackState.SetComboData(nextAttackCombo);
				nextAttackCombo = null;
				break;
			case ECharacterState.SPRINTING:
				newBehaviourState = sprintingMovement;
				break;
			case ECharacterState.HITSTUN:
				newBehaviourState = hitstunState;
				break;
			default:
				newBehaviourState = currentBehaviourState;
				break;
		}

		if (newBehaviourState == null)
			return;
		
		if (!CanChangeThroughStamina(newBehaviourState))
			return;

		currentBehaviourState.OnExit();
		behaviourState = newState;

		currentBehaviourState = newBehaviourState;
		
		StaminaSystem.SetCurrentRecovery(CharacterData.StaminaRegeneration);

		currentBehaviourState.OnEnter();

		if ((currentBehaviourState.GetStateData().AllowedSubstates & substate) == 0)
			ChangeSubstate(ECharacterSubstate.NOTHING);
		
		TimeElapsed = 0;
	}

	public bool CanChangeState(ECharacterState newState)
	{
		StateData currentStateData = currentBehaviourState.GetStateData();
		
		bool canTransition = (((currentStateData.CanTransitionStates & newState) != 0) &&
		                      TimeElapsed > currentStateData.TimeBeforeCanTransition) ||
		                     ((currentStateData.CanAlwaysTransitionStates & newState) != 0);

		return canTransition;
	}

	public bool CanChangeThroughStamina(IStateBehaviour behaviourState)
	{
		StateData newStateData = behaviourState.GetStateData();
		
		bool hasEnoughStamina = StaminaSystem.CurrentStamina >= newStateData.StaminaNeeded || newStateData.StaminaNeeded <= 0;
		bool isTired = StaminaSystem.IsTired && !newStateData.IgnoreBeingTired;
		
		return (hasEnoughStamina && !isTired) || !CharacterData.UsesStamina;
	}
	
	public void ChangePhysicsState(EPhysicsState newState)
	{
		currentPhysicsState.OnExit();
		physicsState = newState;

		switch (newState)
		{
			case EPhysicsState.AIRBORN:
				currentPhysicsState = airbornPhysics;
				ChangeState(ECharacterState.AIRBORN);
				break;
			case EPhysicsState.GROUNDED:
				currentPhysicsState = groundedPhysics;
				ChangeState(ECharacterState.WALKING);
				break;
		}

		currentPhysicsState.OnEnter();
	}

	public void ChangeSubstate(ECharacterSubstate newState)
	{
		if (!CanChangeSubstate(newState))
			return;
		
		currentSubstate.OnExit();
		substate = newState;

		switch (newState)
		{
			case ECharacterSubstate.NOTHING:
				currentSubstate = nothingSubstate;
				break;
			case ECharacterSubstate.BLOCKING:
				currentSubstate = blockingSubstate;
				break;
		}

		currentSubstate.OnEnter();
	}
	
	public bool CanChangeSubstate(ECharacterSubstate newState)
	{
		return (currentBehaviourState.GetStateData().AllowedSubstates & newState) != 0 || newState == ECharacterSubstate.NOTHING;
	}

	public void SetLockedTarget(Transform newTarget)
	{
		LockedTarget = newTarget;
		IsLocked = newTarget != null;
	}

	public void OnDisable()
	{
		currentBehaviourState.OnExit();
	}
}