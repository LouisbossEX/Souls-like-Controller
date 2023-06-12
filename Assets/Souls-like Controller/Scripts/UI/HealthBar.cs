using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider RedHealthBar;
    public Slider YellowHealthBar;
    
    private Coroutine healthBarYellowLerpCoroutine;
    private bool yellowCoroutineRunning = false;

    private HealthSystem healthSystem;
    
    public void Initialize(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;
        
        RedHealthBar.maxValue = healthSystem.MaxHealth;
        YellowHealthBar.maxValue = healthSystem.MaxHealth;
        
        RedHealthBar.value = healthSystem.CurrentHealth;
        YellowHealthBar.value = healthSystem.CurrentHealth;

        healthSystem.OnTakeDamage += DamageFeedback;
    }

    private void DamageFeedback()
    {
        RedHealthBar.value = healthSystem.CurrentHealth;

        SmoothUpdateUI();
    }
    
    private void SmoothUpdateUI()
    {
        if (!yellowCoroutineRunning && gameObject.activeSelf)
            healthBarYellowLerpCoroutine = StartCoroutine(LerpCoroutine(-0.5f, 1f, YellowHealthBar));
    }

    IEnumerator LerpCoroutine(float speed, float delay, Slider healthSlider)
    {
        yellowCoroutineRunning = true;

        yield return new WaitForSeconds(delay);
        
        while (healthSlider.value >= healthSystem.CurrentHealth)
        {
            healthSlider.value += Time.deltaTime * speed * healthSlider.maxValue;
            yield return null;
        }

        healthSlider.value = healthSystem.CurrentHealth;

        yellowCoroutineRunning = false;
    }

    private void OnDestroy()
    {
        healthSystem.OnTakeDamage -= DamageFeedback;
        if (yellowCoroutineRunning)
        {
            StopCoroutine(healthBarYellowLerpCoroutine);
        }
    }
}