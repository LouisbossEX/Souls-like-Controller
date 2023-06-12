using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChanger : MonoBehaviour
{
    [SerializeField] private GameObject character;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.name != character.name)
        {
            other.gameObject.SetActive(false);
            character.transform.position = other.transform.position;
            character.SetActive(true);
            
            FindObjectOfType<HealthSystemUI>().SetCharacter(character.GetComponent<CharacterStatesController>());
            FindObjectOfType<StaminaSystemUI>().SetCharacter(character.GetComponent<CharacterStatesController>());
            FindObjectOfType<UILockedElementsManager>().SetCharacter(character.GetComponent<CharacterStatesController>());
        }
    }
}
