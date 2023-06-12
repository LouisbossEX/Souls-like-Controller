using UnityEngine;

public class BlockingSubstate : ISubstateBehaviour
{
    private CharacterStatesController controller;
    private Animator _animator;
    private MovementRecord movementRecord;
    private BlockingData blockingData;

    private float elapsedTime;

    private bool protectionActive;
    
    public BlockingSubstate(CharacterStatesController controller)
    {
        this.controller = controller;
        _animator = controller.Animator;
        movementRecord = controller.MovementRecord;
        if(controller.leftHandEquipment != null && controller.leftHandEquipment.BlockingData != null)
            blockingData = controller.leftHandEquipment.BlockingData;
    }
    
    public void OnEnter()
    {
        movementRecord.HorizontalSpeedMultiplier -= 1 - blockingData.MovementSpeedMultiplier;
        _animator.SetBool("Blocking", true);
        protectionActive = false;
    }

    public void Update(float deltaTime)
    {
        elapsedTime += deltaTime;

        if (elapsedTime >= controller.leftHandEquipment.BlockingData.TimeToStartBlocking && !protectionActive)
        {
            protectionActive = true;
            controller.HealthSystem.IncreaseMaxPoise(controller.leftHandEquipment.BlockPoise);
        }
    }

    public void OnExit()
    {
        movementRecord.HorizontalSpeedMultiplier += 1 - blockingData.MovementSpeedMultiplier;
        _animator.SetBool("Blocking", false);
        controller.HealthSystem.IncreaseMaxPoise(-controller.leftHandEquipment.BlockPoise);
        protectionActive = false;
    }
}
