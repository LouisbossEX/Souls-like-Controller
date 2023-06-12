using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "NPC/New Dodging Data", fileName = "New" + nameof(NPCDodgingData))]
public class NPCDodgingData : ScriptableObject
{
    public float Cooldown;
    public float EnemyDistanceToDodge;
}
