using UnityEngine;

public class NPCHitstunnedState : INPCStateBehaviour
{
    private NPCController npcController;
    private CharacterStatesController controller;

    public NPCHitstunnedData stateData;

    public NPCHitstunnedState(NPCController npcController, CharacterStatesController controller)
    {
        this.npcController = npcController;
        this.controller = controller;
    }
    
    public void OnEnter()
    {
        
    }

    public void Update(float deltaTime)
    {
        float enemyDistance = Vector3.Distance(npcController.transform.position, npcController.lockedTarget.transform.position);

        if (controller.behaviourState != ECharacterState.HITSTUN)
        {
            if (enemyDistance < stateData.DistanceToDodge && Random.Range(1,101) >= stateData.DodgeChance)
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
        
    }

    public void UpdateStateData()
    {
        stateData = npcController.currentBehaviourData.HitstunnedState;
    }
    
    public float GetRemainingCooldown()
    {
        return 0;
    }
}