using UnityEngine;

public class AttackingState : IStateBehaviour
{
    private CharacterStatesController controller;

    private AttackComboData attackComboData;
    private AttackData currentAttackData;
    private float elapsedTime;
    private MovementRecord movementRecord;

    private float _targetRotation;
    private float _playerTargetRotation;

    private Animator _animator;

    private float _rotationVelocity = 0;
    private float _speed;

    private int comboIndex = -1;
    private float lastAttackTime;

    public AttackingState(CharacterStatesController controller)
    {
        this.controller = controller;
        _animator = controller.Animator;
        movementRecord = controller.MovementRecord;
    }
    
    public void OnEnter()
    {
        if (Time.time - lastAttackTime > currentAttackData.TimeToComboAttack || comboIndex >= attackComboData.AttacksList.Length - 1)
            comboIndex = -1;
        
        comboIndex++;

        currentAttackData = attackComboData.AttacksList[comboIndex];

        elapsedTime = 0;
        lastAttackTime = Time.time;
        
        _animator.SetTrigger(currentAttackData.AnimationID);

        controller.StaminaSystem.DrainStamina(currentAttackData.StaminaCost);
        controller.attacksHurtboxController.StartAttack();
    }

    public void Update(float deltaTime)
    {
        elapsedTime += deltaTime;

        if (elapsedTime < currentAttackData.TimeTillCantRotate)
        {
            Vector2 direction = movementRecord.InputDirection;

            if (controller.IsLocked && currentAttackData.AttackTagetDirection)
            {
                _targetRotation = Quaternion.LookRotation(controller.LockedTarget.position - controller.transform.position, Vector3.up).eulerAngles.y;
            }
            else if (direction.magnitude > 0.1f)
            {
                _targetRotation = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + controller.LookAtObject.transform.eulerAngles.y;
            }
            else
            {
                _targetRotation = controller.transform.eulerAngles.y;
            }
            
            float rotation = Mathf.SmoothDampAngle(controller.transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, currentAttackData.RotationSpeed);

            controller.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

            _playerTargetRotation = rotation;
        }
        else
        {
            _playerTargetRotation = controller.transform.eulerAngles.y;
        }
        
        movementRecord.Movement = Quaternion.Euler(0.0f, _playerTargetRotation, 0.0f) * Vector3.forward;
        movementRecord.Movement.Normalize();
        movementRecord.Movement *= currentAttackData.MovementCurve.Evaluate(elapsedTime) * currentAttackData.MaxSpeed * Time.deltaTime;
        
        if (elapsedTime >= currentAttackData.Duration)
        {
            controller.ChangeState(ECharacterState.WALKING);
        }
    }

    public void OnExit()
    {
        controller.attacksHurtboxController.AttackEnded();
        
        if (elapsedTime <= currentAttackData.TimeBeforeCanTransition)
        {
            comboIndex = -1;
        }
    }
    
    public StateData GetStateData()
    {
        return currentAttackData;
    }

    public AttackData GetCurrentAttackData()
    {
        return currentAttackData;
    }

    public void SetComboData(AttackComboData comboData)
    {
        if (comboData == attackComboData)
            return;
        
        attackComboData = comboData;
        currentAttackData = attackComboData.AttacksList[0];
        comboIndex = -1;
    }
}
