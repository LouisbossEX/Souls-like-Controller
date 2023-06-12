using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Player Blocking/New Player Blocking Data", fileName = "New" + nameof(BlockingData))]
public class BlockingData : ScriptableObject
{
    public string AnimationID;
    public float MovementSpeedMultiplier = 1;
    public float TimeToStartBlocking;
}