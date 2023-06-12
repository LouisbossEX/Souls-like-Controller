using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MovementData))]
public class MovementDataEditor : Editor
{
    private MovementData movementData;
    
    private SerializedProperty minMovementProperty;
    private SerializedProperty speedMultiplierProperty;
    private SerializedProperty accelerationProperty;
    private SerializedProperty decelerationProperty;
    private SerializedProperty faceLockedTargetProperty;
    private SerializedProperty faceLockedTargetWhenIdleProperty;
    private SerializedProperty lockedSpeedMultiplierProperty;

    //State
    private SerializedProperty timeBeforeCanTransitionProperty;
    private SerializedProperty durationProperty;
    private SerializedProperty staminaCostProperty;
    private SerializedProperty staminaRecoveryProperty;
    private SerializedProperty staminaNeededProperty;
    private SerializedProperty ignoreBeingTiredProperty;
    private SerializedProperty canTransitionStatesProperty;
    private SerializedProperty canAlwaysTransitionStatesProperty;
    private SerializedProperty allowedSubstatesProperty;
    private SerializedProperty animationIDProperty;
    
    private MovementData currentTarget;
    
    private GUIStyle titleStyle;
    
    private Vector2 scrollPos = Vector2.zero;
    
    private void OnEnable()
    {
        currentTarget = target as MovementData;
    
        minMovementProperty = serializedObject.FindProperty(nameof(MovementData.MinMovement));
        speedMultiplierProperty = serializedObject.FindProperty(nameof(MovementData.SpeedMultiplier));
        accelerationProperty = serializedObject.FindProperty(nameof(MovementData.Acceleration));
        decelerationProperty = serializedObject.FindProperty(nameof(MovementData.Deceleration));
        faceLockedTargetProperty = serializedObject.FindProperty(nameof(MovementData.FaceLockedTarget));
        faceLockedTargetWhenIdleProperty = serializedObject.FindProperty(nameof(MovementData.FaceLockedTargetWhenIdle));
        lockedSpeedMultiplierProperty = serializedObject.FindProperty(nameof(MovementData.LockedSpeedMultiplier));
        
        //State
        timeBeforeCanTransitionProperty = serializedObject.FindProperty(nameof(MovementData.TimeBeforeCanTransition));
        durationProperty = serializedObject.FindProperty(nameof(MovementData.Duration));
        staminaCostProperty = serializedObject.FindProperty(nameof(MovementData.StaminaCost));
        staminaRecoveryProperty = serializedObject.FindProperty(nameof(MovementData.StaminaRecoveryMultiplier));
        staminaNeededProperty = serializedObject.FindProperty(nameof(MovementData.StaminaNeeded));
        ignoreBeingTiredProperty = serializedObject.FindProperty(nameof(MovementData.IgnoreBeingTired));
        canTransitionStatesProperty = serializedObject.FindProperty(nameof(MovementData.CanTransitionStates));
        canAlwaysTransitionStatesProperty = serializedObject.FindProperty(nameof(MovementData.CanAlwaysTransitionStates));
        allowedSubstatesProperty = serializedObject.FindProperty(nameof(MovementData.AllowedSubstates));
        animationIDProperty = serializedObject.FindProperty(nameof(MovementData.AnimationID));

        titleStyle = new GUIStyle();
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 16;
        titleStyle.normal.textColor = new Color(0.9f, 0.9f, 0.9f);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        
        var boxStyle = new GUIStyle("Box");

        EditorGUILayout.LabelField("Transition To Other States", titleStyle);
        EditorGUILayout.Space(5);

        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(timeBeforeCanTransitionProperty);
        EditorGUILayout.PropertyField(canTransitionStatesProperty);
        EditorGUILayout.PropertyField(canAlwaysTransitionStatesProperty);
        EditorGUILayout.PropertyField(allowedSubstatesProperty);
        EditorGUILayout.PropertyField(durationProperty);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("Stamina", titleStyle);
        EditorGUILayout.Space(5);

        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(staminaCostProperty);
        EditorGUILayout.PropertyField(staminaRecoveryProperty);
        EditorGUILayout.PropertyField(staminaNeededProperty);
        EditorGUILayout.PropertyField(ignoreBeingTiredProperty);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("Animation", titleStyle);
        EditorGUILayout.Space(5);

        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(animationIDProperty);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("Movement", titleStyle);
        EditorGUILayout.Space(5);

        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.Slider(minMovementProperty, 0.0f, 0.5f);
        EditorGUILayout.PropertyField(speedMultiplierProperty);
        EditorGUILayout.PropertyField(accelerationProperty);
        EditorGUILayout.PropertyField(decelerationProperty);
        
        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("When Locked", new GUIStyle(){fontStyle = FontStyle.Bold, normal = new GUIStyleState(){textColor = new Color(0.9f,0.9f,0.9f)}});
        EditorGUILayout.PropertyField(faceLockedTargetWhenIdleProperty, new GUIContent("Face Target When Idle"));
        EditorGUILayout.PropertyField(faceLockedTargetProperty, new GUIContent("Face Target"));
        EditorGUI.BeginDisabledGroup(!faceLockedTargetProperty.boolValue);
        EditorGUILayout.PropertyField(lockedSpeedMultiplierProperty);
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndScrollView();

        serializedObject.ApplyModifiedProperties();
    }

    public void SetMovementData(MovementData data)
    {
        movementData = data;
    }
}
