using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirbornMovement : MovementState
{
    public AirbornMovement(CharacterStatesController controller)
    {
        this.movementData = controller.CharacterData.AirbornMovementData;
        this.controller = controller;
        this.movementRecord = controller.MovementRecord;
        this.playerTransform = controller.transform;
        this.staminaSystem = controller.StaminaSystem;
        this.animator = controller.Animator;

        animIDSpeed = Animator.StringToHash("Speed");
        animIDGrounded = Animator.StringToHash("Grounded");
        animIDSpeedForward = Animator.StringToHash("SpeedForward");
        animIDSpeedRight = Animator.StringToHash("SpeedRight");
        animIDLocked = Animator.StringToHash("Locked");
    }
    
    public override void OnEnter()
    {
	    movementRecord.MovementDirection = new Vector2(movementRecord.Movement.x, movementRecord.Movement.z);
        animator.SetBool(movementData.AnimationID, true);
    }

    public override void Update(float deltaTime)
    {
        Vector2 direction = movementRecord.MovementDirection;
	    
        float targetSpeed = (!controller.IsLocked ? (controller.CharacterData.Speed * movementData.SpeedMultiplier) : (controller.CharacterData.Speed * movementData.LockedSpeedMultiplier)) * direction.magnitude;

        if (direction.magnitude < movementData.MinMovement)
            targetSpeed = 0.0f;

        float speedOffset = 0.1f;
        //float inputMagnitude = _input.AnalogMovement ? _input.Move.magnitude : 1f;
        
        if (movementRecord.HorizontalSpeed < targetSpeed - speedOffset)
        {
	        speed = Mathf.Lerp(movementRecord.HorizontalSpeed, targetSpeed, Time.deltaTime * movementData.Acceleration);
		
	        speed = Mathf.Round(speed * 1000f) / 1000f;
        }
        else if (movementRecord.HorizontalSpeed > targetSpeed + speedOffset)
        {
	        speed = Mathf.Lerp(movementRecord.HorizontalSpeed, targetSpeed, Time.deltaTime * movementData.Deceleration);
		
	        speed = Mathf.Round(speed * 1000f) / 1000f;
        }
        else
        {
	        speed = targetSpeed;
        }

        movementRecord.HorizontalSpeed = speed;

        targetRotation = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
		float rotation = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);

		playerTransform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

		playerTargetRotation = targetRotation;

		movementRecord.Movement = Quaternion.Euler(0.0f, playerTargetRotation, 0.0f) * Vector3.forward;
        movementRecord.Movement.Normalize();
        movementRecord.Movement *= speed * Time.deltaTime;

        if (controller.physicsState == EPhysicsState.GROUNDED)
        {
	        controller.ChangeState(ECharacterState.WALKING);
	        Debug.Log("Change State to Walking");
        }
    }

    public override void OnExit()
    {
	    animator.SetBool("Falling", false);
    }
    
    public override StateData GetStateData()
    {
        return movementData;
    }
}
