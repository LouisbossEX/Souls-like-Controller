using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMovement : MovementState
{
    public GroundMovement(CharacterStatesController controller, MovementData movementData)
    {
        this.movementData = movementData;
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
	    if (controller.physicsState == EPhysicsState.AIRBORN)
	    {
		    controller.ChangeState(ECharacterState.AIRBORN);
	    }
    }

    public override void Update(float deltaTime)
    {
	    Vector2 direction = movementRecord.InputDirection;
		
	    float targetSpeed;
	    
	    if(controller.behaviourState == ECharacterState.SPRINTING)
		    targetSpeed = controller.characterData.Speed * movementData.SpeedMultiplier * movementRecord.HorizontalSpeedMultiplier;
	    else
			targetSpeed = (!controller.IsLocked ? (controller.characterData.Speed * movementData.SpeedMultiplier) : (controller.characterData.Speed * movementData.LockedSpeedMultiplier)) * direction.magnitude * movementRecord.HorizontalSpeedMultiplier;

        if (direction.magnitude < movementData.MinMovement)
            targetSpeed = 0.0f;

        float speedOffset = 0.1f;
        
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
        
		//_animationBlendForward = direction.y * _speed / (controller.characterData.Speed * movementData.SpeedMultiplier);
		//_animationBlendRight = direction.x * _speed / (controller.characterData.Speed * movementData.SpeedMultiplier);
		animationBlendForward = Mathf.Lerp(animationBlendForward, targetSpeed * direction.y, Time.deltaTime * speedChange);
		animationBlendRight = Mathf.Lerp(animationBlendRight, targetSpeed * direction.x, Time.deltaTime * speedChange);
		animationBlend = speed / controller.characterData.Speed;

		Vector2 inputDirection = direction.normalized;
		
		if (controller.IsLocked && (movementData.FaceLockedTarget || (movementData.FaceLockedTargetWhenIdle && targetSpeed == 0)))
		{
			targetRotation = Quaternion.LookRotation(controller.LockedTarget.position - playerTransform.position, Vector3.up).eulerAngles.y;
			float rotation = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);

			playerTransform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

			if (direction.magnitude >= movementData.MinMovement)
				playerTargetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + controller.LookAtObject.eulerAngles.y;

			animator.SetFloat(animIDSpeedForward, animationBlendForward);
			animator.SetFloat(animIDSpeedRight, animationBlendRight);
			animator.SetBool(animIDLocked, true);
		}
        else
        {
			if (direction.magnitude >= movementData.MinMovement)
			{
				targetRotation = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + controller.LookAtObject.eulerAngles.y;
				float rotation = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);

				playerTransform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

				playerTargetRotation = targetRotation;
			}
			animator.SetBool(animIDLocked, false);
        }
		animator.SetFloat(animIDSpeed, animationBlend);

        movementRecord.Movement = Quaternion.Euler(0.0f, playerTargetRotation, 0.0f) * Vector3.forward;
        movementRecord.Movement.Normalize();
        movementRecord.Movement *= speed * Time.deltaTime;
        
        controller.StaminaSystem.DrainStamina(movementData.StaminaCost * deltaTime);
        
        if (controller.StaminaSystem.IsTired && controller.behaviourState == ECharacterState.SPRINTING)
	        controller.ChangeState(ECharacterState.WALKING);
    }

    public override void OnExit()
    {
	    animator.SetFloat(animIDSpeedForward, 0);
	    animator.SetFloat(animIDSpeedRight, 0);
	    animator.SetBool(animIDLocked, false);
    }
    
    public override StateData GetStateData()
    {
	    return movementData;
    }
}
