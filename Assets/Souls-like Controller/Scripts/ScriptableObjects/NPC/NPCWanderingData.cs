using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "NPC/New Wandering Data", fileName = "New" + nameof(NPCWanderingData))]
public class NPCWanderingData : ScriptableObject
{
    public float WanderMaxTime;
    public float EnemyDetectionRange;
}
