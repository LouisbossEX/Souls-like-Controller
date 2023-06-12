using UnityEngine;

public class NPCAttackingState : INPCStateBehaviour
{
    private NPCController npcController;
    private CharacterStatesController controller;
    
    public NPCAttackingData stateData;
    public float remainingCooldown = 0;
    
    public NPCAttackingState(NPCController npcController, CharacterStatesController controller)
    {
        this.npcController = npcController;
        this.controller = controller;
    }
    
    public void OnEnter()
    {
        if (controller.rightHandEquipment.SecondaryAttackCombo != null && Random.Range(0, 6) == 0)
        {
            controller.SetAttackState(controller.rightHandEquipment.SecondaryAttackCombo, EWeaponSlot.RIGHT);
        }
        else
        {
            controller.SetAttackState(controller.rightHandEquipment.PrimaryAttackCombo, EWeaponSlot.RIGHT);
        }
    }

    public void Update(float deltaTime)
    {
        float enemyDistance = Vector3.Distance(npcController.transform.position, npcController.lockedTarget.transform.position);
        
        if (controller.TimeElapsed >= controller.attackState.GetCurrentAttackData().TimeBeforeCanTransition && enemyDistance < 2)
        {
            switch (npcController.Behaviour)
            {
                case ENPCBehaviours.CAUTION:
                    npcController.ChangeState(ENPCStates.DODGING);
                    break;
                case ENPCBehaviours.NEUTRAL:
                    npcController.ChangeState(ENPCStates.ATTACKING);
                    break;
                case ENPCBehaviours.AGGRESIVE:
                    npcController.ChangeState(ENPCStates.ATTACKING);
                    break;
            }
        }
        else if (controller.behaviourState != ECharacterState.ATTACKING)
        {
            npcController.ChangeState(npcController.lockedTarget == null ? ENPCStates.WANDERING : ENPCStates.CHASING);
        }
    }

    public void OnExit()
    {
        remainingCooldown = Time.time + stateData.Cooldown;
    }
    
    public void UpdateStateData()
    {
        stateData = npcController.currentBehaviourData.AttackingState;
    }
    
    public float GetRemainingCooldown()
    {
        return remainingCooldown;
    }
}