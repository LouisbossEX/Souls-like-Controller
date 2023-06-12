using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Player Attack/New Player Attack Data", fileName = "New" + nameof(AttackData))]
public class AttackData : StateData
{
    public float TimeTillCantRotate;
    public float MaxSpeed;
    public float RotationSpeed;
    public bool AttackTagetDirection;
    public float TimeToComboAttack;
    public float DamageMultiplier;
    public float PoiseDamageMultiplier = 1;
    public float HyperArmor;
    public bool TwoHandedAttack;
    public AnimationCurve MovementCurve;
}