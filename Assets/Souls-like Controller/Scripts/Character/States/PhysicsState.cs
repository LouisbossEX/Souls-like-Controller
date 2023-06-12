using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsState
{
    protected CharacterStatesController controller;
    protected PhysicsData physicsData;
    protected MovementRecord movementRecord;
    
    public virtual void OnEnter()
    {
	    
    }

    public virtual void Update(float deltaTime)
    {
        movementRecord.VerticalSpeed -= physicsData.Gravity * deltaTime;
        movementRecord.Movement.y += movementRecord.VerticalSpeed * deltaTime;

        movementRecord.Movement += movementRecord.ImpulseVelocity * deltaTime;
        movementRecord.ImpulseVelocity = Vector3.Lerp(movementRecord.ImpulseVelocity, Vector3.zero, physicsData.ImpulseVelocityDecaySpeed * deltaTime);

        if (movementRecord.ImpulseVelocity.x < 0.01f && movementRecord.ImpulseVelocity.z < 0.01f)
        {
	        movementRecord.IsBeingPushed = false;
        }
    }

    public virtual void OnExit()
    {
        
    }
}
