using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgingState : IStateBehaviour
{
    private CharacterStatesController controller;
    private MovementRecord movementRecord;
    private DodgeData dodgeData;
    private StaminaSystem staminaSystem;
    private float elapsedTime;
    private Animator animator;
    private Transform playerTransform;
    
    private int _animIDDodge;
    private float cameraYaw;
    
    public DodgingState(CharacterStatesController controller)
    {
        this.controller = controller;
        this.movementRecord = controller.MovementRecord;
        this.dodgeData = controller.CharacterData.DodgeData;
        this.staminaSystem = controller.StaminaSystem;
        this.animator = controller.Animator;
        this.playerTransform = controller.transform;
        
        _animIDDodge = Animator.StringToHash(dodgeData.AnimationID);
    }
    
    public void OnEnter()
    {
        movementRecord.DodgeDirection = movementRecord.InputDirection.normalized;
        elapsedTime = 0;
        animator.SetTrigger(dodgeData.AnimationID);
        cameraYaw = controller.LookAtObject.eulerAngles.y;
        
        if(controller.IsLocked)
        {
            animator.SetFloat("DirectionX", movementRecord.DodgeDirection.x);
            animator.SetFloat("DirectionY", movementRecord.DodgeDirection.y);
        }
        else
        {
            animator.SetFloat("DirectionX", 0);
            animator.SetFloat("DirectionY", 1);
        }
        
        controller.StaminaSystem.DrainStamina(dodgeData.StaminaCost);
    }

    public void Update(float deltaTime)
    {
        elapsedTime += deltaTime;
        
        if (elapsedTime > dodgeData.Duration)
        {
            controller.ChangeState(ECharacterState.WALKING);
        }

        if (elapsedTime < dodgeData.TimeBeforeCantRotate)
        {
            movementRecord.DodgeDirection = movementRecord.InputDirection.normalized;
            
            if(controller.IsLocked)
            {
                animator.SetFloat("DirectionX", movementRecord.DodgeDirection.x);
                animator.SetFloat("DirectionY", movementRecord.DodgeDirection.y);
            }
            else
            {
                animator.SetFloat("DirectionX", 0);
                animator.SetFloat("DirectionY", 1);
            }
        }
        
        controller.HealthSystem.SetRollingState(elapsedTime >= dodgeData.TimeBeforeIntangible && elapsedTime <= dodgeData.IntangibleDuration);
        
        float _targetRotation = Mathf.Atan2(movementRecord.DodgeDirection.x, movementRecord.DodgeDirection.y) * Mathf.Rad2Deg + cameraYaw;
		
        if(controller.IsLocked && dodgeData.RotateAroundTarget)
            _targetRotation = Mathf.Atan2(movementRecord.DodgeDirection.x, movementRecord.DodgeDirection.y) * Mathf.Rad2Deg + Quaternion.LookRotation(controller.LockedTarget.position - playerTransform.position, Vector3.up).eulerAngles.y;
        
        if (controller.IsLocked && dodgeData.FaceTarget)
        {
            playerTransform.LookAt(controller.LockedTarget.position);
            playerTransform.rotation = Quaternion.Euler(0.0f, playerTransform.rotation.eulerAngles.y, 0.0f);
            _targetRotation = Mathf.Atan2(movementRecord.DodgeDirection.x, movementRecord.DodgeDirection.y) * Mathf.Rad2Deg + cameraYaw;
        }
        else
        {
            if (movementRecord.DodgeDirection.magnitude < 0.1f)
                _targetRotation = controller.transform.eulerAngles.y;
            
            playerTransform.rotation = Quaternion.Euler(0.0f, _targetRotation, 0.0f);
        }
        
        movementRecord.Movement = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        movementRecord.Movement.Normalize();
        movementRecord.Movement *= dodgeData.MovementCurve.Evaluate(elapsedTime) * dodgeData.MaxSpeed * Time.deltaTime;
    }

    public void OnExit()
    {
        
    }
    
    public StateData GetStateData()
    {
        return dodgeData;
    }
}
