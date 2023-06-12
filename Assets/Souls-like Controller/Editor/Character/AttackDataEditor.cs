using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AttackData))]
public class AttackDataEditor : Editor
{
    private AttackData attackData;

    private SerializedProperty timeTillCantRotateProperty;
    private SerializedProperty maxSpeedProperty;
    private SerializedProperty rotationSpeedProperty;
    private SerializedProperty timeToComboAttackProperty;
    private SerializedProperty damageMultiplierProperty;
    private SerializedProperty attackTagetDirectionProperty;
    private SerializedProperty movementCurveProperty;
    private SerializedProperty poiseDamageMultiplierProperty;
    private SerializedProperty hyperArmorProperty;
    private SerializedProperty twoHandedAttackProperty;
    
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
    
    private Rect rect;
    private GUIStyle titleStyle;

    private AttackData currentTarget;
    
    private void OnEnable()
    {
        currentTarget = target as AttackData;
    
        timeTillCantRotateProperty = serializedObject.FindProperty(nameof(AttackData.TimeTillCantRotate));
        maxSpeedProperty = serializedObject.FindProperty(nameof(AttackData.MaxSpeed));
        rotationSpeedProperty = serializedObject.FindProperty(nameof(AttackData.RotationSpeed));
        timeToComboAttackProperty = serializedObject.FindProperty(nameof(AttackData.TimeToComboAttack));
        damageMultiplierProperty = serializedObject.FindProperty(nameof(AttackData.DamageMultiplier));
        attackTagetDirectionProperty = serializedObject.FindProperty(nameof(AttackData.AttackTagetDirection));
        movementCurveProperty = serializedObject.FindProperty(nameof(AttackData.MovementCurve));
        poiseDamageMultiplierProperty = serializedObject.FindProperty(nameof(AttackData.PoiseDamageMultiplier));
        hyperArmorProperty = serializedObject.FindProperty(nameof(AttackData.HyperArmor));
        twoHandedAttackProperty = serializedObject.FindProperty(nameof(AttackData.TwoHandedAttack));
        
        //State
        timeBeforeCanTransitionProperty = serializedObject.FindProperty(nameof(AttackData.TimeBeforeCanTransition));
        durationProperty = serializedObject.FindProperty(nameof(AttackData.Duration));
        staminaCostProperty = serializedObject.FindProperty(nameof(AttackData.StaminaCost));
        staminaRecoveryProperty = serializedObject.FindProperty(nameof(AttackData.StaminaRecoveryMultiplier));
        staminaNeededProperty = serializedObject.FindProperty(nameof(AttackData.StaminaNeeded));
        ignoreBeingTiredProperty = serializedObject.FindProperty(nameof(AttackData.IgnoreBeingTired));
        canTransitionStatesProperty = serializedObject.FindProperty(nameof(AttackData.CanTransitionStates));
        canAlwaysTransitionStatesProperty = serializedObject.FindProperty(nameof(AttackData.CanAlwaysTransitionStates));
        allowedSubstatesProperty = serializedObject.FindProperty(nameof(AttackData.AllowedSubstates));
        animationIDProperty = serializedObject.FindProperty(nameof(AttackData.AnimationID));
        
        titleStyle = new GUIStyle();
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 16;
        titleStyle.normal.textColor = new Color(0.9f, 0.9f, 0.9f);
    }

    private Vector2 scrollPos = Vector2.zero;
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var boxStyle = new GUIStyle("Box");

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        
        EditorGUILayout.LabelField("Transition To Other States", titleStyle);
        EditorGUILayout.Space(5f);
        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(timeBeforeCanTransitionProperty);
        EditorGUILayout.PropertyField(canTransitionStatesProperty);
        EditorGUILayout.PropertyField(canAlwaysTransitionStatesProperty);
        EditorGUILayout.PropertyField(allowedSubstatesProperty);
        EditorGUILayout.PropertyField(durationProperty);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(15);
        
        EditorGUILayout.LabelField("Stamina", titleStyle);
        EditorGUILayout.Space(5f);
        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(staminaCostProperty);
        EditorGUILayout.PropertyField(staminaRecoveryProperty);
        EditorGUILayout.PropertyField(staminaNeededProperty);
        EditorGUILayout.PropertyField(ignoreBeingTiredProperty);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(15);
        
        EditorGUILayout.LabelField("Animation", titleStyle);
        EditorGUILayout.Space(5f);
        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(animationIDProperty);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(15);
        
        EditorGUILayout.LabelField("Movement", titleStyle);
        EditorGUILayout.Space(5f);
        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(timeTillCantRotateProperty);
        EditorGUILayout.PropertyField(maxSpeedProperty);
        EditorGUILayout.PropertyField(rotationSpeedProperty);
        EditorGUILayout.PropertyField(attackTagetDirectionProperty);
        
        rect = new Rect(0,0,durationProperty.floatValue, 1);

        if (movementCurveProperty.animationCurveValue.keys.Length == 0)
        {
            movementCurveProperty.animationCurveValue = AnimationCurve.Linear(0,0,durationProperty.floatValue, 1);
        }
        
        //var keyframes = movementCurveProperty.animationCurveValue.keys;
        //for (int i = 0; i < keyframes.Length; i++)
        //{
        //    float previousDuration = keyframes[keyframes.Length - 1].time / attackData.MovementDuration;
        //
        //    keyframes[i].time = keyframes[i].time / previousDuration;
        //}
        //movementCurveProperty.animationCurveValue.keys = keyframes;
        //attackData.MovementCurve.keys[1].time = 0.5f;
        //movementCurveProperty.animationCurveValue.keys[1].value = 0.5f;
        //Debug.Log(movementCurveProperty.animationCurveValue.keys[1].time);
        EditorGUILayout.CurveField(movementCurveProperty, Color.red, rect);
        
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(15);
        
        EditorGUILayout.LabelField("Attack", titleStyle);
        EditorGUILayout.Space(5f);
        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(timeToComboAttackProperty);
        EditorGUILayout.PropertyField(damageMultiplierProperty);
        EditorGUILayout.PropertyField(poiseDamageMultiplierProperty);
        EditorGUILayout.PropertyField(hyperArmorProperty);
        EditorGUILayout.PropertyField(twoHandedAttackProperty);
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
        
        GUILayout.FlexibleSpace();
        
        serializedObject.ApplyModifiedProperties();
    }

    public void SetAttackData(AttackData data)
    {
        attackData = data;
    }
}
