using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class NPCController : MonoBehaviour
{
    public NavMeshAgent followTarget;
    public CharacterStatesController controller;
    public Transform lookAtObject;

    public CharacterStatesController lockedTarget;

    public float dodgeChance = 30;

    private INPCStateBehaviour wanderingState;
    private INPCStateBehaviour chaseState;
    private INPCStateBehaviour watchingState;
    private INPCStateBehaviour dodgingState;
    private INPCStateBehaviour attackingState;
    private INPCStateBehaviour hitstunnedState;

    private INPCStateBehaviour currentNpcBehaviour;
    public ENPCStates currentNpcState;

    public bool IsLocking => lockedTarget != null;

    public ECharacterType TargetType;
    
    public float NextDodgeTime = 0;
    public float NextStateTime = 0;

    public float attackRange = 2f;
    
    public ENPCBehaviours Behaviour;
    public NPCBehaviourData aggresiveBehaviourData;
    public NPCBehaviourData neutralBehaviourData;
    public NPCBehaviourData cautionBehaviourData;

    public NPCBehaviourData currentBehaviourData;
    
    private HealthSystem healthSystem;
    
    private void Awake()
    {
        controller = GetComponent<CharacterStatesController>();
        healthSystem = GetComponent<HealthSystem>();
        
        var go = Instantiate(new GameObject());
        go.name = gameObject.name + " AI Look At Target";

        lookAtObject = go.transform;
        controller.LookAtObject = lookAtObject;
        
        var navMeshAgentGo = Instantiate(new GameObject());
        navMeshAgentGo.AddComponent<NavMeshAgent>();
        navMeshAgentGo.name = gameObject.name + " Nav Mesh Agent";

        followTarget = navMeshAgentGo.GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        currentNpcState = ENPCStates.WANDERING;
        
        wanderingState = new NPCWanderState(this, controller);
        chaseState = new NPCChaseState(this, controller);
        watchingState = new NPCWatchingState(this, controller);
        dodgingState = new NPCDodgingState(this, controller);
        attackingState = new NPCAttackingState(this, controller);
        hitstunnedState = new NPCHitstunnedState(this, controller);

        currentNpcBehaviour = wanderingState;
        currentNpcBehaviour.OnEnter();

        ChangeBehaviour(ENPCBehaviours.AGGRESIVE);
    }

    void Update()
    {
        if (lockedTarget != null)
            followTarget.transform.position = lockedTarget.transform.position;
        
        Vector3 newPosition = transform.position - (followTarget.transform.position - transform.position).normalized;
        lookAtObject.position = newPosition;
        lookAtObject.LookAt(followTarget.transform);
        
        currentNpcBehaviour.Update(Time.deltaTime);

        var currentStaminaPercentage = controller.StaminaSystem.CurrentStamina/controller.StaminaSystem.MaxStamina*100;
        var currentHealthPercentage = controller.HealthSystem.CurrentHealth/controller.HealthSystem.MaxHealth*100;

        switch (Behaviour)
        {
            case ENPCBehaviours.CAUTION:
                if ((currentStaminaPercentage + currentHealthPercentage * 3) / 4 > currentBehaviourData.MaxAggresionFactor)
                {
                    ChangeBehaviour(ENPCBehaviours.NEUTRAL);
                }
                break;
            
            case ENPCBehaviours.NEUTRAL:
                if ((currentStaminaPercentage + currentHealthPercentage * 3) / 4 < currentBehaviourData.MinAggresionFactor)
                {
                    ChangeBehaviour(ENPCBehaviours.CAUTION);
                }
                else if ((currentStaminaPercentage + currentHealthPercentage * 3) / 4 > currentBehaviourData.MaxAggresionFactor)
                {
                    ChangeBehaviour(ENPCBehaviours.AGGRESIVE);
                }
                break;
            
            case ENPCBehaviours.AGGRESIVE:
                if ((currentStaminaPercentage + currentHealthPercentage) / 2 < currentBehaviourData.MinAggresionFactor)
                {
                    ChangeBehaviour(ENPCBehaviours.NEUTRAL);
                }
                break;
        }
        
        
        if (lockedTarget != null && !lockedTarget.HealthSystem.IsAlive)
        {
            SetLockedTarget(null);
            ChangeState(ENPCStates.WANDERING);
        }
    }

    public void SetLockedTarget(CharacterStatesController newLockedEnemy)
    {
        lockedTarget = newLockedEnemy;
        controller.SetLockedTarget(lockedTarget != null ? newLockedEnemy.transform : null);
    }

    public void ChangeState(ENPCStates newState)
    {
        INPCStateBehaviour newNpcBehaviour;
        
        switch (newState)
        {
            case ENPCStates.WANDERING:
                newNpcBehaviour = wanderingState;
                break;
            case ENPCStates.ATTACKING:
                newNpcBehaviour = attackingState;
                if (newNpcBehaviour.GetRemainingCooldown() > Time.time)
                {
                    ChangeState(ENPCStates.WATCHING);
                    return;
                }
                break;
            case ENPCStates.CHASING:
                newNpcBehaviour = chaseState;
                break;
            case ENPCStates.DODGING:
                newNpcBehaviour = dodgingState;
                if (newNpcBehaviour.GetRemainingCooldown() > Time.time)
                {
                    ChangeState(ENPCStates.CHASING);
                    return;
                }
                break;
            case ENPCStates.WATCHING:
                newNpcBehaviour = watchingState;
                if (newNpcBehaviour.GetRemainingCooldown() > Time.time)
                {
                    ChangeState(ENPCStates.CHASING);
                    return;
                }
                break;
            case ENPCStates.HITSTUNNED:
                newNpcBehaviour = hitstunnedState;
                break;
            default:
                newNpcBehaviour = currentNpcBehaviour;
                break;
        }
        
        currentNpcBehaviour.OnExit();
        currentNpcState = newState;

        currentNpcBehaviour = newNpcBehaviour;

        currentNpcBehaviour.OnEnter();
    }

    public void ChangeBehaviour(ENPCBehaviours newBehaviour)
    {
        Behaviour = newBehaviour;

        switch (Behaviour)
        {
            case ENPCBehaviours.CAUTION:
                currentBehaviourData = cautionBehaviourData;
                break;
            case ENPCBehaviours.NEUTRAL:
                currentBehaviourData = neutralBehaviourData;
                break;
            case ENPCBehaviours.AGGRESIVE:
                currentBehaviourData = aggresiveBehaviourData;
                break;
        }

        wanderingState.UpdateStateData();
        chaseState.UpdateStateData();
        watchingState.UpdateStateData();
        dodgingState.UpdateStateData();
        attackingState.UpdateStateData();
        hitstunnedState.UpdateStateData();
    }

    private void GetAttacked()
    {
        ChangeState(ENPCStates.HITSTUNNED);
    }
    
    private void OnEnable()
    {
        if (healthSystem != null)
        {
            healthSystem.OnTakeDamage += GetAttacked;
        }
    }

    private void OnDisable()
    {
        if (healthSystem != null)
        {
            healthSystem.OnTakeDamage -= GetAttacked;
        }
    }
}
