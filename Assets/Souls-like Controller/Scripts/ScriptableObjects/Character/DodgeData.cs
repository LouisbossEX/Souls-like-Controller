using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Player Dodge/New Player Dodge Data", fileName = "New" + nameof(DodgeData))]
public class DodgeData : StateData
{
    public float MaxSpeed;
    
    public AnimationCurve MovementCurve;
    
    public float TimeBeforeCantRotate;
    public bool FaceTarget;
    public bool RotateAroundTarget;

    public float TimeBeforeIntangible;
    public float IntangibleDuration;
    //Could add
    //public float RotationSpeed;
}