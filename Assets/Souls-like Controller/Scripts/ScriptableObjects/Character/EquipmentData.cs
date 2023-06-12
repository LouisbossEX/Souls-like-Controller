using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum EEquipmentAction
{
    NONE,
    PRIMARY_ATTACK,
    SECONDARY_ATTACK,
    BLOCK
}

[CreateAssetMenu(menuName = "Equipment Data/New Equipment Data", fileName = "New" + nameof(EquipmentData))]
public class EquipmentData : ScriptableObject
{
    public EEquipmentAction PrimaryRightHand;
    public EEquipmentAction SecondaryRightHand;
    public EEquipmentAction PrimaryLeftHand;
    public EEquipmentAction SecondaryLeftHand;

    //public bool TwoHandedOnly;
    
    public float BaseAttack;
    public float AttackStrengthScaling;
    public float PoiseDamage;
    public AttackComboData PrimaryAttackCombo;
    public AttackComboData SecondaryAttackCombo;

    public float BlockDamageReduction;
    public float BlockPoise;
    public BlockingData BlockingData;

    public bool ShowHitboxes = false;
    //public float BlockStrenghtScaling;
}
