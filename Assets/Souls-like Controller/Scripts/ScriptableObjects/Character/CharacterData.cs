using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Player Data/New Player Data", fileName = "New" + nameof(CharacterData))]
public class CharacterData : ScriptableObject
{
    public float Health;
    public bool UseRagdollOnDeath;
    public string DeathAnimationID;
    public bool UsesStamina;
    public float MaxStamina;
    public float StaminaRegeneration;
    public float StaminaRegenerationDelay;
    public float NotTiredPercentage;
    public bool StaminaCanGoBelowZero;
    public float Strength;
    public float Speed;
    
    public float Poise;
    public float PoiseRegeneration;
    public float PoiseRegenerationDelay;
    
    [FormerlySerializedAs("walkingMovementData")] public MovementData WalkingMovementData;
    [FormerlySerializedAs("sprintingMovementData")] public MovementData SprintingMovementData;
    [FormerlySerializedAs("airbornMovementData")] public MovementData AirbornMovementData;

    public DodgeData DodgeData;

    public PhysicsData GroundedPhysicsData;
    public PhysicsData AirbornPhysicsData;

    public EquipmentData StartingRightHandEquipment;
    public EquipmentData StartingLeftHandEquipment;

    public HitstunData HitstunData;
}