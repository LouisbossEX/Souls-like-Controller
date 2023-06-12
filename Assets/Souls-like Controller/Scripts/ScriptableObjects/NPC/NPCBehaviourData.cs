using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "NPC/New Behaviour Data", fileName = "New" + nameof(NPCBehaviourData))]
public class NPCBehaviourData : ScriptableObject
{
    //State
    public ENPCBehaviours Behaviour;
    public float MinAggresionFactor;            //Need at least X to enter this state
    public float MaxAggresionFactor;            //If you have less than X stamina you leave the state

    public NPCAttackingData AttackingState;
    public NPCChaseData ChaseState;
    public NPCHitstunnedData HitstunnedState;
    public NPCWanderingData WanderingState;
    public NPCWatchingData WatchingState;
    public NPCDodgingData DodgingState;
}
