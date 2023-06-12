using UnityEngine;

public interface INPCStateBehaviour
{
    void OnEnter();
    void Update(float deltaTime);
    void OnExit();
    void UpdateStateData();
    float GetRemainingCooldown();
}