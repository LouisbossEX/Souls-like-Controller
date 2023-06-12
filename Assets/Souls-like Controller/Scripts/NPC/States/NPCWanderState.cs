using UnityEngine;

public class NPCWanderState : INPCStateBehaviour
{
    private NPCController npcController;
    private CharacterStatesController controller;

    private float nextSearchTime;

    public NPCWanderingData stateData;
    
    public NPCWanderState(NPCController npcController, CharacterStatesController controller)
    {
        this.npcController = npcController;
        this.controller = controller;
    }
    
    public void OnEnter()
    {
        nextSearchTime = 0;
        controller.MovementRecord.InputDirection = Vector2.zero;
    }

    public void Update(float deltaTime)
    {
        if (Time.time >= nextSearchTime)
        {
            nextSearchTime = Time.time + 1;

            float closestEnemyDistance = 0;
            CharacterStatesController closestEnemy = null;
            
            foreach (var character in GameObject.FindObjectsOfType<CharacterStatesController>())
            {
                if ((character.CharacterType & npcController.TargetType) == 0 || character == controller || !character.HealthSystem.IsAlive)
                    continue;

                float distance = Vector3.Distance(character.transform.position, controller.transform.position);

                if (distance < stateData.EnemyDetectionRange && closestEnemyDistance < distance)
                {
                    closestEnemyDistance = distance;
                    closestEnemy = character;
                }
            }

            if (closestEnemy != null)
            {
                npcController.SetLockedTarget(closestEnemy);
                npcController.ChangeState(ENPCStates.CHASING);
            }
        }
    }

    public void OnExit()
    {
        
    }
    
    public void UpdateStateData()
    {
        stateData = npcController.currentBehaviourData.WanderingState;
    }

    public float GetRemainingCooldown()
    {
        return 0;
    }
}