using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AttacksHurtboxController : MonoBehaviour
{
    private List<HurtboxCollider> rightHandEquipmentColliders = new List<HurtboxCollider>();
    private List<HurtboxCollider> leftHandEquipmentColliders = new List<HurtboxCollider>();
    
    private CharacterStatesController controller;
    
    private bool hurtboxActive = false;
    private bool hyperArmorActive = false;

    private bool attackOver = false;

    private EWeaponSlot currentAttackSlot;
    
    private void Awake()
    {
        controller = GetComponentInParent<CharacterStatesController>();
        
        foreach (var hurtbox in GetComponentsInChildren<HurtboxCollider>())
        {
            if(hurtbox.WeaponSlot == EWeaponSlot.RIGHT)
                rightHandEquipmentColliders.Add(hurtbox);
            else if(hurtbox.WeaponSlot == EWeaponSlot.LEFT)
                leftHandEquipmentColliders.Add(hurtbox);
        }
    }

    public void ActivateHurtbox()
    {
        if (hurtboxActive || attackOver)
            return;

        if (controller.attackState.GetCurrentAttackData().TwoHandedAttack)
        {
            foreach (var collider in rightHandEquipmentColliders)
            {
                collider.ActivateCollider();
            }
            
            foreach (var collider in leftHandEquipmentColliders)
            {
                collider.ActivateCollider();
            }
        }
        else
        {
            foreach (var collider in currentAttackSlot == EWeaponSlot.RIGHT ? rightHandEquipmentColliders : leftHandEquipmentColliders)
            {
                collider.ActivateCollider();
            }
        }

        hurtboxActive = true;
    }
    
    public void DeactivateHurtbox()
    {
        if (!hurtboxActive || attackOver)
            return;
        
        if (controller.attackState.GetCurrentAttackData().TwoHandedAttack)
        {
            foreach (var collider in rightHandEquipmentColliders)
            {
                collider.DeactivateCollider();
            }
            
            foreach (var collider in leftHandEquipmentColliders)
            {
                collider.DeactivateCollider();
            }
        }
        else
        {
            foreach (var collider in currentAttackSlot == EWeaponSlot.RIGHT ? rightHandEquipmentColliders : leftHandEquipmentColliders)
            {
                collider.DeactivateCollider();
            }
        }

        hurtboxActive = false;
    }
    
    public void ActivateHyperArmor()
    {
        if (hyperArmorActive || attackOver)
            return;
        
        controller.HealthSystem.IncreaseMaxPoise(controller.attackState.GetCurrentAttackData().HyperArmor);

        hyperArmorActive = true;
    }
    
    public void DeactivateHyperArmor()
    {
        if (!hyperArmorActive || attackOver)
            return;
        
        controller.HealthSystem.IncreaseMaxPoise(-controller.attackState.GetCurrentAttackData().HyperArmor);
        
        hyperArmorActive = false;
    }

    public void SetWeapon(EWeaponSlot weaponSlot)
    {
        currentAttackSlot = weaponSlot;
    }
    
    public void StartAttack()
    {
        attackOver = false;
    }

    public void AttackEnded()
    {
        DeactivateHyperArmor();
        DeactivateHurtbox();
        
        attackOver = true;
    }
}