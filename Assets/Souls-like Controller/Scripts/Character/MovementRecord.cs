using UnityEngine;

public class MovementRecord
{
    public Vector2 InputDirection;
    public float HorizontalSpeedMultiplier = 1f;
    public float HorizontalSpeed;
    public Vector3 Movement;
    public Vector2 MovementDirection;
    public Vector3 DodgeDirection;
    public float VerticalSpeed;
    public bool IsBeingPushed;
    public Vector3 ImpulseVelocity = Vector3.zero;
    //public Vector3 JumpDirection;
    //public Vector3 AirVelocity = Vector3.zero;
}