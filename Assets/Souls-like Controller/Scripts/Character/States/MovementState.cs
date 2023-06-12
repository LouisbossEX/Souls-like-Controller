using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementState : IStateBehaviour
{
    protected MovementData movementData;
    protected MovementRecord movementRecord;
    protected Transform playerTransform;
    protected StaminaSystem staminaSystem;
    protected Animator animator;
    protected CharacterStatesController controller;
    
    protected float rotationSmoothTime = 0.09f;

    protected float targetRotation = 0.0f;
    protected float playerTargetRotation = 0.0f;
    protected float rotationVelocity;
    protected float speed;
    
    protected float speedChange = 10;
    
    protected float animationBlend;
    protected float animationBlendForward;
    protected float animationBlendRight;
    
    protected int animIDSpeed;
    protected int animIDGrounded;
    protected int animIDSpeedForward;
    protected int animIDSpeedRight;
    protected int animIDLocked;
    
    protected ClampedFloatValue timeMoving;

    public abstract void OnEnter();
    public abstract void Update(float deltaTime);
    public abstract void OnExit();
    public abstract StateData GetStateData();
}
