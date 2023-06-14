using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum EWeaponSlot
{
    RIGHT,
    LEFT
}

[RequireComponent(typeof(BoxCollider))]
public class HurtboxCollider : MonoBehaviour
{
    private CharacterStatesController controller;
    private BoxCollider weaponCollider;

    [SerializeField] private GameObject debugPrefab;
    private GameObject debugCollider;

    public EWeaponSlot WeaponSlot;

    private List<GameObject> charactersHit = new List<GameObject>();
    
    private void Awake()
    {
        controller = GetComponentInParent<CharacterStatesController>();
        gameObject.layer = LayerMask.NameToLayer("Hurtbox");
        weaponCollider = GetComponent<BoxCollider>();
    }
    
    void Start()
    {
        weaponCollider.isTrigger = true;
        weaponCollider.enabled = false;

        if (debugPrefab == null)
        {
            Debug.LogWarning("Debug Prefab not serialized");
        }
        
        debugCollider = Instantiate(debugPrefab, transform);
        debugCollider.GetComponent<HurtboxDebug>().Initialize(weaponCollider);
    }

    void Update()
    {
        if (WeaponSlot == EWeaponSlot.RIGHT && controller.rightHandEquipment != null)
        {
            debugCollider.SetActive(controller.rightHandEquipment.ShowHitboxes);
        }
        else if (WeaponSlot == EWeaponSlot.LEFT && controller.leftHandEquipment != null)
        {
            debugCollider.SetActive(controller.leftHandEquipment.ShowHitboxes);
        }
        else
        {
            debugCollider.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.GetComponent<CharacterStatesController>()?.CharacterType & controller.CanAttackType) == 0 || other.gameObject == gameObject)
            return;

        bool enemyAlreadyHit = false;
        
        foreach (var characterHit in charactersHit)
        {
            if (characterHit == other.gameObject)
                enemyAlreadyHit = true;
        }
        
        HealthSystem healthSystem = other.GetComponent<HealthSystem>();
        if (healthSystem != null && !enemyAlreadyHit)
        {
            EquipmentData weaponUsed = controller.rightHandEquipment;
            
            if (WeaponSlot == EWeaponSlot.RIGHT)
                weaponUsed = controller.rightHandEquipment;
            else if (WeaponSlot == EWeaponSlot.LEFT)
                weaponUsed = controller.leftHandEquipment;
            
            healthSystem.TakeDamage(controller.CharacterData, weaponUsed, controller.attackState.GetCurrentAttackData());
            
            charactersHit.Add(other.gameObject);
            
            //StartCoroutine(DamageAnimationStopCoroutine(weaponUsed));
        }
    }
    /*
    private IEnumerator DamageAnimationStopCoroutine(EquipmentData equipmentData)
    {
        controller.Animator.speed = 0;
        yield return new WaitForSeconds(0.05f);
        controller.Animator.speed = 1;
    }
    */
    public void ActivateCollider()
    {
        weaponCollider.enabled = true;
        charactersHit.Clear();
    }

    public void DeactivateCollider()
    {
        weaponCollider.enabled = false;
    }
}