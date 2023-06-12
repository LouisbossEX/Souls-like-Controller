using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "NPC/New Chase Data", fileName = "New" + nameof(NPCChaseData))]
public class NPCChaseData : ScriptableObject
{
    public float Cooldown;
    public int DodgeChance;
    public float DistanceToAttackMultiplier = 1;
    public float DistanceToDodge;
    public float DistanceToWatch;
    public float DodgeAttemptCooldown;
}
