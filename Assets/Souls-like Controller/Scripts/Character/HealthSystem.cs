using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class HealthSystem : MonoBehaviour
{
    private CharacterStatesController controller;
    private CharacterData characterData;
    
    private float currentHealth;
    private float maxHealth;
    
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsAlive => CurrentHealth > 0;
    
    private float currentPoise;
    private float maxPoise;
    private float timeTillPoiseRegeneration = 0;

    public Action OnTakeDamage;
    [HideInInspector] public bool IsRolling;

    public Action OnDeath;
    private CharacterController characterController;
    [SerializeField] private MonoBehaviour[] scriptsToDestroyOnDeath;
    private Animator animator;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
        controller = GetComponent<CharacterStatesController>();

        characterData = controller.CharacterData;
        maxHealth = characterData.Health;
        currentHealth = MaxHealth;

        maxPoise = characterData.Poise;
        currentPoise = maxPoise;
    }

    private void Update()
    {
        if (Time.time >= timeTillPoiseRegeneration)
        {
            currentPoise = Mathf.Clamp(currentPoise + characterData.PoiseRegeneration * Time.deltaTime, 0, maxPoise);
        }
        
        //if (gameObject.tag == "Player")
        //    Debug.Log(currentPoise);
    }

    public void TakeDamage(CharacterData characterData, EquipmentData weaponData, AttackData attackData)
    {
        if (IsRolling)
            return;

        float enemyAttackDamage = weaponData.BaseAttack +
                                  characterData.Strength * weaponData.AttackStrengthScaling *
                                  attackData.DamageMultiplier;
        

        timeTillPoiseRegeneration = Time.time + characterData.PoiseRegenerationDelay;
        
        if (controller.substate == ECharacterSubstate.BLOCKING)
        {
            currentHealth -= enemyAttackDamage * (100 - controller.leftHandEquipment.BlockDamageReduction)/100;
            controller.StaminaSystem.DrainStamina(weaponData.PoiseDamage * 3 * (100 - controller.leftHandEquipment.BlockDamageReduction)/100);

            if (controller.StaminaSystem.CurrentStamina <= 0)
            {
                controller.ChangeState(ECharacterState.HITSTUN);
                currentPoise = maxPoise;
            }
        }
        else
        {
            currentHealth -= enemyAttackDamage;
            currentPoise -= weaponData.PoiseDamage;
        }
        
        OnTakeDamage?.Invoke();
        
        if (CurrentHealth <= 0)
        {
            controller.ChangeState(ECharacterState.DEAD);
            OnDeath?.Invoke();

            if (!this.characterData.UseRagdollOnDeath)
                animator.SetTrigger(this.characterData.DeathAnimationID);
            
            foreach (var script in scriptsToDestroyOnDeath)
            {
                script.enabled = false;
            }

            characterController.enabled = false;
        }
        else if (currentPoise <= 0)
        {
            currentPoise = maxPoise;
            controller.ChangeState(ECharacterState.HITSTUN);
        }
    }
    
    public void TakeDamage(float value)
    {
        if (IsRolling)
            return;
        
        currentHealth -= value;
        
        controller.ChangeState(ECharacterState.HITSTUN);

        OnTakeDamage?.Invoke();
        
        if (CurrentHealth <= 0)
        {
            controller.ChangeState(ECharacterState.DEAD);
            OnDeath?.Invoke();
            
            foreach (var script in scriptsToDestroyOnDeath)
            {
                script.enabled = false;
            }

            characterController.enabled = false;
        }
    }

    public void SetRollingState(bool value)
    {
        IsRolling = value;
    }
    
    public void CallbackOnDeath(Action callback)
    {
        OnDeath += callback;
    }

    public void IncreaseMaxPoise(float value)
    {
        maxPoise += value;
        currentPoise += value;
    }
}