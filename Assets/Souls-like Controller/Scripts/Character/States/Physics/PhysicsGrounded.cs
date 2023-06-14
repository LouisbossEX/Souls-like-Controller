using UnityEngine;

public class PhysicsGrounded : PhysicsState
{
	public float FallTimeout = 0.15f;
    
	private float _fallTimeoutDelta;

    public PhysicsGrounded(CharacterStatesController controller)
    {
        this.controller = controller;
        this.physicsData = controller.CharacterData.GroundedPhysicsData;
        movementRecord = controller.MovementRecord;
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        if ((controller.collisionFlags & CollisionFlags.Below) == 0)
        {
            _fallTimeoutDelta += Time.deltaTime;

            if (_fallTimeoutDelta >= FallTimeout)
            {
                controller.ChangePhysicsState(EPhysicsState.AIRBORN);
            }
        }
        else
        {
            movementRecord.VerticalSpeed = -1.0f;
            _fallTimeoutDelta = 0;
        }
    }
}
