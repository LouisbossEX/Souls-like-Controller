using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaSystemUI : MonoBehaviour
{
    private StaminaSystem staminaSystem;
    [SerializeField] private Slider staminaBar;
    private RectTransform staminaBarRect;
    
    void Start()
    {
        staminaBarRect = staminaBar.GetComponent<RectTransform>();
        
        foreach (var controllers in FindObjectsOfType<CharacterStatesController>())
        {
            if (controllers.gameObject.tag == "Player")
            {
                SetCharacter(controllers);
                break;
            }
        }
    }

    void Update()
    {
        staminaBar.value = staminaSystem.CurrentStamina;
    }
    
    public void SetCharacter(CharacterStatesController newController)
    {
        if (newController == null)
        {
            Destroy(gameObject);
            return;
        }
        if (!newController.CharacterData.UsesStamina)
        {
            Destroy(gameObject);
            return;
        }
        
        staminaSystem = newController.StaminaSystem;
        staminaBarRect.sizeDelta = new Vector2(staminaSystem.MaxStamina * 3, staminaBarRect.sizeDelta.y);
        staminaBar.maxValue = staminaSystem.MaxStamina;
    }
}
