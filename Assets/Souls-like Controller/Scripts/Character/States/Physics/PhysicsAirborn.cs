using UnityEngine;

public class PhysicsAirborn : PhysicsState
{
    public PhysicsAirborn(CharacterStatesController controller)
    {
        this.controller = controller;
        this.physicsData = controller.characterData.AirbornPhysicsData;
        movementRecord = controller.MovementRecord;
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        
        if ((controller.collisionFlags & CollisionFlags.Above) != 0 && movementRecord.VerticalSpeed > 0.0f)
            movementRecord.VerticalSpeed = 0.0f;
        
        if ((controller.collisionFlags & CollisionFlags.Below) != 0)
            controller.ChangePhysicsState(EPhysicsState.GROUNDED);
    }
}