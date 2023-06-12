using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxDebug : MonoBehaviour
{
    [SerializeField] private Material inactiveMaterial;
    [SerializeField] private Material activeMaterial;

    private MeshRenderer meshRenderer;

    private BoxCollider weaponCollider;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Initialize(BoxCollider weaponCollider)
    {
        this.weaponCollider = weaponCollider;
        
        transform.localPosition = weaponCollider.center;
        transform.localScale = weaponCollider.size;
        
        gameObject.SetActive(false);

        meshRenderer.material = inactiveMaterial;
    }

    void Update()
    {
        if (weaponCollider.enabled)
        {
            meshRenderer.material = activeMaterial;
        }
        else
        {
            meshRenderer.material = inactiveMaterial;
        }
    }
}
