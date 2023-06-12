using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "NPC/New Attacking Data", fileName = "New" + nameof(NPCAttackingData))]
public class NPCAttackingData : ScriptableObject
{
    public float Cooldown;
}
