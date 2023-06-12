using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "NPC/New Hitstunned Data", fileName = "New" + nameof(NPCHitstunnedData))]
public class NPCHitstunnedData : ScriptableObject
{
    public int DodgeChance;
    [FormerlySerializedAs("EnemyDistanceToDodge")] public float DistanceToDodge;
}
