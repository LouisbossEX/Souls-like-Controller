using UnityEngine;

public class NPCWatchingState : INPCStateBehaviour
{
    private NPCController npcController;
    private CharacterStatesController controller;

    private float returnToChaseTime;

    private Vector2 cautionInputDirection = Vector2.zero;
    private Vector2 approachInputDirection = Vector2.zero;
    private Vector2 aggresiveInputDirection = Vector2.zero;
    
    public NPCWatchingData stateData;
    public float remainingCooldown = 0;

    public NPCWatchingState(NPCController npcController, CharacterStatesController controller)
    {
        this.npcController = npcController;
        this.controller = controller;
    }
    
    public void OnEnter()
    {
        cautionInputDirection = new Vector2(Random.Range(-0.5f, 0.5f), 1);
        approachInputDirection = new Vector2(Random.Range(0, 2) == 0 ? -1 : 1, Random.Range(-0.25f, 0.25f));
        aggresiveInputDirection = new Vector2(Random.Range(0, 2) == 0 ? -1 : 1, Random.Range(0f, 0.5f));

        returnToChaseTime = Time.time + Random.Range(stateData.WatchingMinTime, stateData.WatchingMaxTime);
    }

    public void Update(float deltaTime)
    {
        float enemyDistance = Vector3.Distance(npcController.lockedTarget.transform.position, controller.transform.position);

        switch (npcController.Behaviour)
        {
            case ENPCBehaviours.CAUTION:
                controller.MovementRecord.InputDirection = Vector2.Lerp(controller.MovementRecord.InputDirection, cautionInputDirection, Time.deltaTime * 4f);
                break;
            
            case ENPCBehaviours.NEUTRAL:
                controller.MovementRecord.InputDirection = Vector2.Lerp(controller.MovementRecord.InputDirection, approachInputDirection, Time.deltaTime * 4f);
                break;
            
            case ENPCBehaviours.AGGRESIVE:
                controller.MovementRecord.InputDirection = Vector2.Lerp(controller.MovementRecord.InputDirection, aggresiveInputDirection, Time.deltaTime * 4f);
                break;
        }
        
        if (Time.time > returnToChaseTime)
        {
            if (Random.Range(0, 101) < stateData.ReenterStateChance)
            {
                npcController.ChangeState(ENPCStates.WATCHING);
            }
            else
            {
                npcController.ChangeState(ENPCStates.CHASING);
            }
        }
        else if (enemyDistance < stateData.EnemyDistanceToChase)
        {
            npcController.ChangeState(ENPCStates.CHASING);
        }
    }

    public void OnExit()
    {
        remainingCooldown = Time.time + stateData.Cooldown;
    }
    
    public void UpdateStateData()
    {
        stateData = npcController.currentBehaviourData.WatchingState;
    }
    
    public float GetRemainingCooldown()
    {
        return remainingCooldown;
    }
}