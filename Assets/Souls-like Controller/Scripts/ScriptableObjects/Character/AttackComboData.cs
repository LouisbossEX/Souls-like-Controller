using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Player Attack/New Player Attack Combo Data", fileName = "New" + nameof(AttackComboData))]
public class AttackComboData : ScriptableObject
{
    public AttackData[] AttacksList;
}
