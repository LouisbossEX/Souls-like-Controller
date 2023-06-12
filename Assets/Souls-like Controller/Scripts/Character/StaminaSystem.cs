using System;
using UnityEngine;
using UnityEngine.UI;

public class StaminaSystem
{
    private float maxStamina;
    private float currentStamina;
    private float currentRecovery;
    private float noLongerTired;
    private float timeToRegenerateAgain = 0;
    private bool tired = false;
    private CharacterData characterData;
    public bool IsTired => tired;
    public float CurrentStamina => currentStamina;
    public float MaxStamina => maxStamina;
    
    public StaminaSystem(CharacterData characterData)
    {
        this.characterData = characterData;
        maxStamina = characterData.MaxStamina;
        noLongerTired = characterData.NotTiredPercentage;
        currentStamina = maxStamina;
    }

    public void Update(float deltaTime)
    {
        if (!characterData.UsesStamina)
            return;

        if (Time.time >= timeToRegenerateAgain)
        {
            currentStamina += currentRecovery * deltaTime;
        
            if(!characterData.StaminaCanGoBelowZero)
                currentStamina = Math.Clamp(currentStamina, 0, maxStamina);
            else if (currentStamina >= maxStamina)
                currentStamina = maxStamina;
        }
        
        if (currentStamina <= 0)
            tired = true;
        else if (currentStamina >= maxStamina * noLongerTired / 100)
            tired = false;
    }

    public void SetCurrentRecovery(float newRecovery)
    {
        currentRecovery = newRecovery;
    }
    
    public void SetMaxStamina(float value)
    {
        maxStamina = value;
    }

    public void DrainStamina(float value)
    {
        currentStamina -= value;

        if(value > 0)
            timeToRegenerateAgain = Time.time + characterData.StaminaRegenerationDelay;
        
        if(!characterData.StaminaCanGoBelowZero)
            currentStamina = Math.Clamp(currentStamina, 0, maxStamina);
    }
}