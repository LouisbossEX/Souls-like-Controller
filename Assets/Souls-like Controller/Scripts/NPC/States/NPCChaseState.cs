using UnityEngine;

public class NPCChaseState : INPCStateBehaviour
{
    private NPCController npcController;
    private CharacterStatesController controller;

    private float nextWatchTime = 0;
    private float nextDodgeAttemptTime = 0;
    
    public NPCChaseData stateData;
    public float remainingCooldown = 0;
    
    public NPCChaseState(NPCController npcController, CharacterStatesController controller)
    {
        this.npcController = npcController;
        this.controller = controller;
    }
    
    public void OnEnter()
    {
        switch (npcController.Behaviour)
        {
            case ENPCBehaviours.CAUTION:
                controller.MovementRecord.InputDirection = new Vector2(Random.Range(-1f, 1f), 1);
                break;
            case ENPCBehaviours.NEUTRAL:
                controller.MovementRecord.InputDirection = new Vector2(0, 1);
                break;
            case ENPCBehaviours.AGGRESIVE:
                controller.MovementRecord.InputDirection = new Vector2(0, 1);
                break;
        }
    }

    public void Update(float deltaTime)
    {
        //controller.TryingToRun = controller.StaminaSystem.GetCurrentStamina() >= 0.7 * controller.StaminaSystem.GetMaxStamina();
        controller.TryingToRun = npcController.Behaviour == ENPCBehaviours.AGGRESIVE;

        float enemyDistance = Vector3.Distance(npcController.transform.position, npcController.lockedTarget.transform.position);
        
        if (enemyDistance < stateData.DistanceToDodge && npcController.lockedTarget.behaviourState == ECharacterState.ATTACKING && Time.time > nextDodgeAttemptTime)
        {
            if (Random.Range(0, 101) < stateData.DodgeChance)
            {
                npcController.ChangeState(ENPCStates.DODGING);
            }
            else
            {
                nextDodgeAttemptTime = Time.time + stateData.DodgeAttemptCooldown;
            }
        }
        else if (enemyDistance < npcController.attackRange * stateData.DistanceToAttackMultiplier)
        {
            npcController.ChangeState(ENPCStates.ATTACKING);
        }
        else if (enemyDistance > stateData.DistanceToWatch && Time.time > nextWatchTime)
        {
            npcController.ChangeState(ENPCStates.WATCHING);
        }
        else if (enemyDistance > 15f)
        {
            npcController.ChangeState(ENPCStates.WANDERING);
        }
    }

    public void OnExit()
    {
        controller.TryingToRun = false;
        remainingCooldown = Time.time + stateData.Cooldown;
    }
    
    public void UpdateStateData()
    {
        stateData = npcController.currentBehaviourData.ChaseState;
    }
    
    public float GetRemainingCooldown()
    {
        return remainingCooldown;
    }
}