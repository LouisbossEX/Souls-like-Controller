using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollActivator : MonoBehaviour
{
    private CharacterStatesController controller;
    private Animator animator;

    private Rigidbody[] rigidbodies;
    private HealthSystem healthSystem;
    private LockableTarget lockableTarget;

    public void Awake()
    {
        controller = GetComponent<CharacterStatesController>();
        healthSystem = GetComponent<HealthSystem>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    void Start()
    {
        animator = controller.Animator;
        
        foreach (var VARIABLE in rigidbodies)
        {
            VARIABLE.gameObject.layer = LayerMask.NameToLayer("RagdollCollider");
        }
        
        SetState(false);
        healthSystem.CallbackOnDeath(() => SetState(true));
    }

    public void SetState(bool enabled)
    {
        bool isKinematic = !enabled;
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = isKinematic;
        }

        animator.enabled = !enabled;
    }
}
