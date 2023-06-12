using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Player Movement/New Player Movement Data", fileName = "New" + nameof(MovementData))]
public class MovementData : StateData
{
    [Range(0.0f,0.5f)]
    public float MinMovement;
    public float SpeedMultiplier;
    public float Acceleration;
    public float Deceleration;
    public bool FaceLockedTarget;
    public bool FaceLockedTargetWhenIdle;
    public float LockedSpeedMultiplier;
}