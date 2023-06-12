using UnityEngine;

public class HitstunState : IStateBehaviour
{
    private CharacterStatesController controller;

    private HitstunData hitstunData;
    private float elapsedTime;
    private MovementRecord movementRecord;

    private float _targetRotation;
    private float _playerTargetRotation;

    private Animator _animator;

    private float _speed;

    private bool hitstunOver = true;

    private int stunsInARow = 0;
    
    public HitstunState(CharacterStatesController controller)
    {
        this.controller = controller;
        _animator = controller.Animator;
        movementRecord = controller.MovementRecord;
        hitstunData = controller.characterData.HitstunData;
    }
    
    public void OnEnter()
    {
        elapsedTime = 0;
        _animator.SetTrigger(hitstunData.AnimationID);

        movementRecord.HorizontalSpeed = 0;
        movementRecord.Movement = Vector2.zero;

        if (!hitstunOver)
        {
            stunsInARow++;
            elapsedTime = hitstunData.Duration / 1 + stunsInARow;
        }
        
        hitstunOver = false;
    }

    public void Update(float deltaTime)
    {
        elapsedTime += deltaTime;

        if (elapsedTime >= hitstunData.TimeBeforeCanTransition)
        {
            controller.ChangeState(ECharacterState.WALKING);
            hitstunOver = true;
            stunsInARow = 0;
        }
    }

    public void OnExit()
    {
        
    }

    public StateData GetStateData()
    {
        return hitstunData;
    }
}