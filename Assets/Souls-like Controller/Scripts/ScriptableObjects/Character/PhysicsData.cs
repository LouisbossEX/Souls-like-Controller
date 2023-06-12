using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Player Physics/New Player Physics Data", fileName = "New" + nameof(PhysicsData))]
public class PhysicsData : ScriptableObject
{
    public float ImpulseVelocityDecaySpeed;
    public float Gravity;
}