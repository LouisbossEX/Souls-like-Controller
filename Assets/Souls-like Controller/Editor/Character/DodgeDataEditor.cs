using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DodgeData))]
public class DodgeDataEditor : Editor
{
    private DodgeData dodgeData;

    private SerializedProperty timeBeforeCantRotateProperty;
    private SerializedProperty speedProperty;
    private SerializedProperty faceLockedTargetProperty;
    private SerializedProperty rotateAroundTargetProperty;
    private SerializedProperty timeBeforeIntangibleProperty;
    private SerializedProperty intangibleDurationProperty;
    private SerializedProperty movementCurveProperty;

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
    
    private DodgeData currentTarget;
    
    private Rect rect;
    private GUIStyle titleStyle;
    
    private Vector2 scrollPos = Vector2.zero;
    
    private void OnEnable()
    {
        currentTarget = target as DodgeData;
    
        timeBeforeCantRotateProperty = serializedObject.FindProperty(nameof(DodgeData.TimeBeforeCantRotate));
        speedProperty = serializedObject.FindProperty(nameof(DodgeData.MaxSpeed));
        faceLockedTargetProperty = serializedObject.FindProperty(nameof(DodgeData.FaceTarget));
        rotateAroundTargetProperty = serializedObject.FindProperty(nameof(DodgeData.RotateAroundTarget));
        timeBeforeIntangibleProperty = serializedObject.FindProperty(nameof(DodgeData.TimeBeforeIntangible));
        intangibleDurationProperty = serializedObject.FindProperty(nameof(DodgeData.IntangibleDuration));
        movementCurveProperty = serializedObject.FindProperty(nameof(DodgeData.MovementCurve));

        //State
        timeBeforeCanTransitionProperty = serializedObject.FindProperty(nameof(DodgeData.TimeBeforeCanTransition));
        durationProperty = serializedObject.FindProperty(nameof(DodgeData.Duration));
        staminaCostProperty = serializedObject.FindProperty(nameof(DodgeData.StaminaCost));
        staminaRecoveryProperty = serializedObject.FindProperty(nameof(DodgeData.StaminaRecoveryMultiplier));
        staminaNeededProperty = serializedObject.FindProperty(nameof(DodgeData.StaminaNeeded));
        ignoreBeingTiredProperty = serializedObject.FindProperty(nameof(DodgeData.IgnoreBeingTired));
        canTransitionStatesProperty = serializedObject.FindProperty(nameof(DodgeData.CanTransitionStates));
        canAlwaysTransitionStatesProperty = serializedObject.FindProperty(nameof(DodgeData.CanAlwaysTransitionStates));
        allowedSubstatesProperty = serializedObject.FindProperty(nameof(DodgeData.AllowedSubstates));
        animationIDProperty = serializedObject.FindProperty(nameof(DodgeData.AnimationID));

        titleStyle = new GUIStyle();
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 16;
        titleStyle.normal.textColor = new Color(0.9f, 0.9f, 0.9f);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        
        EditorGUILayout.LabelField("Transition To Other States", titleStyle);
        EditorGUILayout.Space(5);
        
        var boxStyle = new GUIStyle("Box");
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
        EditorGUILayout.PropertyField(timeBeforeCantRotateProperty);
        EditorGUILayout.PropertyField(speedProperty);
        
        rect = new Rect(0,0,durationProperty.floatValue, 1);
        if (movementCurveProperty.animationCurveValue.keys.Length == 0)
        {
            movementCurveProperty.animationCurveValue = AnimationCurve.Linear(0,0,durationProperty.floatValue, 1);
        }
        EditorGUILayout.CurveField(movementCurveProperty, Color.red, rect);
        
        EditorGUILayout.LabelField("When Locked", new GUIStyle(){fontStyle = FontStyle.Bold, normal = new GUIStyleState(){textColor = new Color(0.9f,0.9f,0.9f)}});
        EditorGUILayout.PropertyField(faceLockedTargetProperty);
        EditorGUILayout.PropertyField(rotateAroundTargetProperty);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("Dodge", titleStyle);
        EditorGUILayout.Space(5);

        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(timeBeforeIntangibleProperty);
        EditorGUILayout.PropertyField(intangibleDurationProperty);
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndScrollView();
        
        serializedObject.ApplyModifiedProperties();
    }

    public void SetDodgeData(DodgeData data)
    {
        dodgeData = data;
    }
}
