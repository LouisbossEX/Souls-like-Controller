using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerInputsManager : MonoBehaviour
{
    private bool cursorLocked = true;

    public Action OnLockEnemy;
    
    private MovementRecord movementRecord;
    private CharacterStatesController controller;

    private float TimeBeforeRunning = 0.2f;
    [HideInInspector] public bool TryingToRun = false;
    private Coroutine dodgeToRunningCoroutine = null;

    private Input rightHandPrimaryAction;
    private Input rightHandSecondaryAction;
    private Input dodge;
    private Input run;
    private Input block;
    private Input unblock;

    private Input inputToExecute = null;

    private float elapsedTime = 0;
    private float timeToResetInput = 0.75f;
    
    private void Awake()
    {
        controller = GetComponent<CharacterStatesController>();
    }

    private void Start()
    {
        movementRecord = controller.MovementRecord;

        rightHandPrimaryAction = new Input(1,  () => ExecuteEquipmentAction(EWeaponSlot.RIGHT, 1), ECharacterState.ATTACKING);
        rightHandSecondaryAction = new Input(1,  () => ExecuteEquipmentAction(EWeaponSlot.RIGHT, 2), ECharacterState.ATTACKING);
        dodge = new Input(2, () => controller.ChangeState(ECharacterState.DODGING), ECharacterState.DODGING);
        run = new Input(3,  () => controller.ChangeState(ECharacterState.SPRINTING), ECharacterState.SPRINTING);
        block = new Input(4, () => ExecuteEquipmentAction(EWeaponSlot.LEFT, 1), ECharacterSubstate.BLOCKING);
        unblock = new Input(5, () => ExecuteEquipmentAction(EWeaponSlot.LEFT, 1), ECharacterSubstate.NOTHING);
    }

    private void ExecuteEquipmentAction(EWeaponSlot weaponSlot, int value)
    {
        EquipmentData equipmentData = weaponSlot == EWeaponSlot.RIGHT ? controller.rightHandEquipment : controller.leftHandEquipment;
        
        if (equipmentData != null)
        {
            if (value == 1)
                ExecuteAction(weaponSlot == EWeaponSlot.RIGHT ? equipmentData.PrimaryRightHand : equipmentData.PrimaryLeftHand, equipmentData, weaponSlot);
            else if (value == 2)
                ExecuteAction(weaponSlot == EWeaponSlot.RIGHT ? equipmentData.SecondaryRightHand : equipmentData.SecondaryLeftHand, equipmentData, weaponSlot);
        }
    }
    
    private void ExecuteAction(EEquipmentAction action, EquipmentData equipmentData, EWeaponSlot weaponSlot)
    {
        switch (action)
        {
            case EEquipmentAction.PRIMARY_ATTACK:
                controller.SetAttackState(equipmentData.PrimaryAttackCombo, weaponSlot);
                break;
            case EEquipmentAction.SECONDARY_ATTACK:
                controller.SetAttackState(equipmentData.SecondaryAttackCombo, weaponSlot);
                break;
            case EEquipmentAction.BLOCK:
                controller.ChangeSubstate(controller.substate != ECharacterSubstate.BLOCKING ? ECharacterSubstate.BLOCKING : ECharacterSubstate.NOTHING);
                break;
        }
    }
    
    private void Update()
    {
        if (inputToExecute == null)
            return;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= timeToResetInput)
        {
            inputToExecute = null;
        }
        else if (inputToExecute.IsStateInput)
        {
            if (controller.CanChangeState(inputToExecute.State))
            {
                inputToExecute.InputAction?.Invoke();
                inputToExecute = null;
            }
        }
        else if (!inputToExecute.IsStateInput)
        {
            if (controller.CanChangeSubstate(inputToExecute.Substate))
            {
                inputToExecute.InputAction?.Invoke();
                inputToExecute = null;
            }
        }
        
    }

    public void OnMove(InputValue input)
    {
        //MoveInput(value.Get<Vector2>());
        Vector2 _inputDirection = input.Get<Vector2>();

        if (_inputDirection.magnitude > 1)
            _inputDirection.Normalize();

        movementRecord.InputDirection = _inputDirection;
    }

    public void OnLock(InputValue value)
    {
        OnLockEnemy?.Invoke();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private void ChangeInputToExecute(Input input)
    {
        if (inputToExecute == null)
        {
            inputToExecute = input;
            elapsedTime = 0;
            return;
        }
        else if (input.Priority <= inputToExecute.Priority)
        {
            inputToExecute = input;
            elapsedTime = 0;
        }
    }
    
    public void OnRightPrimary()
    {
        ChangeInputToExecute(rightHandPrimaryAction);
    }
    
    public void OnRightSecondary()
    {
        ChangeInputToExecute(rightHandSecondaryAction);
    }

    public void OnDodgeDown()
    {
        dodgeToRunningCoroutine = StartCoroutine(OnDodgeInput());
    }

    public void OnDodgeUp()
    {
        if (TryingToRun == false)
        {
            StopCoroutine(dodgeToRunningCoroutine);
            
            ChangeInputToExecute(dodge);
        }
        else
        {
            controller.TryingToRun = false;
            TryingToRun = false;
        }
    }

    public void OnLeftPrimaryDown()
    {
        ChangeInputToExecute(block);
    }

    public void OnLeftPrimaryUp()
    {
        ChangeInputToExecute(unblock);
    }
    
    IEnumerator OnDodgeInput()
    {
        yield return new WaitForSeconds(TimeBeforeRunning);
        controller.TryingToRun = true;
        TryingToRun = true;
    }

    public void OnRestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void OnExit()
    {
        Application.Quit();
    }
}

class Input
{
    public int Priority;
    public Action InputAction;
    public ECharacterState State;
    public ECharacterSubstate Substate;

    public bool IsStateInput;
    
    public Input(int priority, Action inputAction, ECharacterState state)
    {
        Priority = priority;
        InputAction = inputAction;
        State = state;

        IsStateInput = true;
    }
    
    public Input(int priority, Action inputAction, ECharacterSubstate substate)
    {
        Priority = priority;
        InputAction = inputAction;
        Substate = substate;

        IsStateInput = false;
    }
}