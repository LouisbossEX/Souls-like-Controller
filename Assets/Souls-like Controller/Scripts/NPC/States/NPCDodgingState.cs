using UnityEngine;
public class NPCDodgingState : INPCStateBehaviour
{
    private NPCController npcController;
    private CharacterStatesController controller;
    
    private float nextDodgeTime;
    private float dodgeCooldown = 5f;
    
    public NPCDodgingData stateData;
    public float remainingCooldown = 0;
    
    public NPCDodgingState(NPCController npcController, CharacterStatesController controller)
    {
        this.npcController = npcController;
        this.controller = controller;
    }
    
    public void OnEnter()
    {
        switch (npcController.Behaviour)
        {
            case ENPCBehaviours.CAUTION:
                controller.MovementRecord.InputDirection = new Vector2(Random.Range(-1f, 1f), -1);

                break;
            case ENPCBehaviours.NEUTRAL:
                controller.MovementRecord.InputDirection = new Vector2(Random.Range(0, 2) == 0 ? -1 : 1, Random.Range(-0.2f, 0.2f));

                break;
            case ENPCBehaviours.AGGRESIVE:
                controller.MovementRecord.InputDirection = new Vector2(Random.Range(-1f, 1f), 1);

                break;
        }
        
        npcController.NextDodgeTime = Time.time + dodgeCooldown;
        controller.ChangeState(ECharacterState.DODGING);
    }

    public void Update(float deltaTime)
    {
        if (controller.behaviourState != ECharacterState.DODGING)
        {
            float enemyDistance = Vector3.Distance(npcController.transform.position, npcController.lockedTarget.transform.position);

            if (npcController.lockedTarget.behaviourState == ECharacterState.ATTACKING && enemyDistance <= stateData.EnemyDistanceToDodge)
            {
                npcController.ChangeState(ENPCStates.DODGING);
            }
            else
            {
                npcController.ChangeState(ENPCStates.CHASING);
            }
        }
    }

    public void OnExit()
    {
        remainingCooldown = Time.time + stateData.Cooldown;
    }
    
    public void UpdateStateData()
    {
        stateData = npcController.currentBehaviourData.DodgingState;
    }
    
    public float GetRemainingCooldown()
    {
        return remainingCooldown;
    }
}