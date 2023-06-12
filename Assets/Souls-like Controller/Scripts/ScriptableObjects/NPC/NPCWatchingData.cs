using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "NPC/New Watching Data", fileName = "New" + nameof(NPCWatchingData))]
public class NPCWatchingData : ScriptableObject
{
    public float Cooldown;
    public float EnemyDistanceToChase;

    public int ReenterStateChance;
    public float WatchingMinTime;
    public float WatchingMaxTime;
}
