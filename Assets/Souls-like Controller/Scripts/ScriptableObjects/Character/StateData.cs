using UnityEngine;
using UnityEngine.Serialization;

public class StateData : ScriptableObject
{
    public float TimeBeforeCanTransition; //If 0 then disable CanTransitionStates flags 
    public ECharacterState CanTransitionStates;
    public ECharacterState CanAlwaysTransitionStates;
    public ECharacterSubstate AllowedSubstates;
    public float Duration;
    public float StaminaCost;
    public float StaminaRecoveryMultiplier;
    public float StaminaNeeded;
    public bool IgnoreBeingTired = true;
    public string AnimationID;
}