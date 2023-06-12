using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LockableTarget : MonoBehaviour
{
    public RectTransform[] UIElements;
    public Transform[] Roots;
    public bool IsAlive;
    private UILockedElementsManager lockedElementsManager;
    private HealthSystem healthSystem;

    private List<CameraController> lockedCameras = new List<CameraController>();
    public Transform CameraRoot;

    public List<RectTransform> elementsCreated = new List<RectTransform>();
    public List<Transform> rootsCreated = new List<Transform>();
    
    private void Awake()
    {
        lockedElementsManager = FindObjectOfType<UILockedElementsManager>();
        healthSystem = GetComponent<HealthSystem>();
        
        lockedElementsManager.LockableTargets.Add(this);
        
        if (CameraRoot == null)
            CameraRoot = transform;
    }

    private void Start()
    {
        IsAlive = true;
        
        for (int i = 0; i < UIElements.Length; i++)
        {
            RectTransform UIElement = Instantiate(UIElements[i]);
            UIElement.SetParent(lockedElementsManager.transform);
            UIElement.localScale = Vector3.one;
            elementsCreated.Add(UIElement);
            rootsCreated.Add(Roots[i]);
            
            //lockedElementsManager.EnemyElements.Add(UIElement);
            //lockedElementsManager.EnemyRoots.Add(Roots[i]);

            var healthBar = UIElement.GetComponent<HealthBar>();
            if (healthBar != null)
            {
                healthBar.Initialize(healthSystem);
            }
        }
        
        if(healthSystem != null)
            healthSystem.CallbackOnDeath(Death);
    }

    public void AddLockedCamera(CameraController camera)
    {
        lockedCameras.Add(camera);
        
        foreach (var VARIABLE in elementsCreated)
        {
            VARIABLE.gameObject.SetActive(true);
        }
    }

    public void RemoveLockedCamera(CameraController camera)
    {
        lockedCameras.Remove(camera);
        
        foreach (var VARIABLE in elementsCreated)
        {
            VARIABLE.gameObject.SetActive(false);
        }
    }
    
    private void Death()
    {
        IsAlive = false;

        foreach (var camera in lockedCameras)
        {
            camera.AssignLockOnTarget(null);
        }
        
        for (int i = 0; i < elementsCreated.Count; i++)
        {
            lockedElementsManager.RemoveEnemyElement(this);
        }
        
        HideElements();
    }

    public void HideElements()
    {
        foreach (var VARIABLE in UIElements)
        {
            VARIABLE.gameObject.SetActive(false);
        }
    }
}
