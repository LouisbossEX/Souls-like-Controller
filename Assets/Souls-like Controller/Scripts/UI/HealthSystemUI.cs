using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthSystemUI : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    private RectTransform healthBarRedSlider;
    private RectTransform healthBarYellowSlider;
    
    void Start()
    {
        healthBarRedSlider = healthBar.RedHealthBar.GetComponent<RectTransform>();
        healthBarYellowSlider = healthBar.YellowHealthBar.GetComponent<RectTransform>();
        
        foreach (var controllers in FindObjectsOfType<CharacterStatesController>())
        {
            if (controllers.gameObject.tag == "Player")
            {
                SetCharacter(controllers);
                break;
            }
        }
    }

    public void SetCharacter(CharacterStatesController newController)
    {
        if (newController == null)
        {
            healthBar.gameObject.SetActive(false);
            return;
        }
        
        healthBar.Initialize(newController.HealthSystem);

        healthBarRedSlider.sizeDelta = new Vector2(newController.HealthSystem.MaxHealth, healthBarRedSlider.sizeDelta.y);
        healthBarYellowSlider.sizeDelta = new Vector2(newController.HealthSystem.MaxHealth, healthBarYellowSlider.sizeDelta.y);
    }
}
