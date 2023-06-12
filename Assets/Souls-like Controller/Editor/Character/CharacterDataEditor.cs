using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterData))]
public class CharacterDataEditor : Editor
{
    private CharacterData characterData;

    private SerializedProperty healthProperty;
    private SerializedProperty ragdollOnDeathProperty;
    private SerializedProperty deathAnimationIDProperty;
    private SerializedProperty speedProperty;
    private SerializedProperty strengthProperty;
    private SerializedProperty usesStaminaProperty;
    private SerializedProperty maxStaminaProperty;
    private SerializedProperty staminaRegenerationProperty;
    private SerializedProperty staminaRegenerationDelayProperty;
    private SerializedProperty notTiredPercentageProperty;
    private SerializedProperty staminaCanGoBelowZeroProperty;

    private SerializedProperty poiseProperty;
    private SerializedProperty poiseRegenerationProperty;
    private SerializedProperty poiseRegenerationDelayProperty;
    
    private SerializedProperty walkingStateProperty;
    private SerializedProperty sprintingStateProperty;
    private SerializedProperty airbornStateZeroProperty;
    private SerializedProperty dodgeDataProperty;
    private SerializedProperty groundPhysicsDataProperty;
    private SerializedProperty airbornPhysicsDataProperty;
    private SerializedProperty hitstunDataProperty;
    
    private SerializedProperty rightHandEquipmentProperty;
    private SerializedProperty leftHandEquipmentProperty;
    
    private CharacterData currentTarget;
    private GUIStyle titleStyle;

    private Vector2 scrollPos = Vector2.zero;
    
    private void OnEnable()
    {
        currentTarget = target as CharacterData;

        healthProperty = serializedObject.FindProperty(nameof(CharacterData.Health));
        ragdollOnDeathProperty = serializedObject.FindProperty(nameof(CharacterData.UseRagdollOnDeath));
        deathAnimationIDProperty = serializedObject.FindProperty(nameof(CharacterData.DeathAnimationID));
        speedProperty = serializedObject.FindProperty(nameof(CharacterData.Speed));
        strengthProperty = serializedObject.FindProperty(nameof(CharacterData.Strength));
        usesStaminaProperty = serializedObject.FindProperty(nameof(CharacterData.UsesStamina));
        maxStaminaProperty = serializedObject.FindProperty(nameof(CharacterData.MaxStamina));
        staminaRegenerationProperty = serializedObject.FindProperty(nameof(CharacterData.StaminaRegeneration));
        staminaRegenerationDelayProperty = serializedObject.FindProperty(nameof(CharacterData.StaminaRegenerationDelay));
        notTiredPercentageProperty = serializedObject.FindProperty(nameof(CharacterData.NotTiredPercentage));
        staminaCanGoBelowZeroProperty = serializedObject.FindProperty(nameof(CharacterData.StaminaCanGoBelowZero));
        
        poiseProperty = serializedObject.FindProperty(nameof(CharacterData.Poise));
        poiseRegenerationProperty = serializedObject.FindProperty(nameof(CharacterData.PoiseRegeneration));
        poiseRegenerationDelayProperty = serializedObject.FindProperty(nameof(CharacterData.PoiseRegenerationDelay));
        
        walkingStateProperty = serializedObject.FindProperty(nameof(CharacterData.WalkingMovementData));
        sprintingStateProperty = serializedObject.FindProperty(nameof(CharacterData.SprintingMovementData));
        airbornStateZeroProperty = serializedObject.FindProperty(nameof(CharacterData.AirbornMovementData));
        dodgeDataProperty = serializedObject.FindProperty(nameof(CharacterData.DodgeData));
        groundPhysicsDataProperty = serializedObject.FindProperty(nameof(CharacterData.GroundedPhysicsData));
        airbornPhysicsDataProperty = serializedObject.FindProperty(nameof(CharacterData.AirbornPhysicsData));
        hitstunDataProperty = serializedObject.FindProperty(nameof(CharacterData.HitstunData));
        
        rightHandEquipmentProperty = serializedObject.FindProperty(nameof(CharacterData.StartingRightHandEquipment));
        leftHandEquipmentProperty = serializedObject.FindProperty(nameof(CharacterData.StartingLeftHandEquipment));
        
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

        EditorGUILayout.LabelField("Stats", titleStyle);
        EditorGUILayout.Space(5f);
        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(healthProperty);
        EditorGUILayout.PropertyField(ragdollOnDeathProperty);
        EditorGUI.BeginDisabledGroup(ragdollOnDeathProperty.boolValue);
        EditorGUILayout.PropertyField(deathAnimationIDProperty);
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.PropertyField(speedProperty);
        EditorGUILayout.PropertyField(strengthProperty);
        EditorGUILayout.PropertyField(usesStaminaProperty);
        EditorGUI.BeginDisabledGroup(!usesStaminaProperty.boolValue);
        EditorGUILayout.PropertyField(maxStaminaProperty);
        EditorGUILayout.PropertyField(staminaRegenerationProperty);
        EditorGUILayout.PropertyField(staminaRegenerationDelayProperty);
        EditorGUILayout.PropertyField(notTiredPercentageProperty);
        EditorGUILayout.PropertyField(staminaCanGoBelowZeroProperty);
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.PropertyField(poiseProperty);
        EditorGUILayout.PropertyField(poiseRegenerationProperty);
        EditorGUILayout.PropertyField(poiseRegenerationDelayProperty);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(15);
        
        EditorGUILayout.LabelField("Starting Equipment", titleStyle);
        EditorGUILayout.Space(5f);
        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(rightHandEquipmentProperty);
        EditorGUILayout.PropertyField(leftHandEquipmentProperty);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(15);
        
        EditorGUILayout.LabelField("States", titleStyle);
        EditorGUILayout.Space(5f);
        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(walkingStateProperty);
        EditorGUILayout.PropertyField(sprintingStateProperty);
        EditorGUILayout.PropertyField(airbornStateZeroProperty);
        EditorGUILayout.PropertyField(dodgeDataProperty);
        EditorGUILayout.PropertyField(groundPhysicsDataProperty);
        EditorGUILayout.PropertyField(airbornPhysicsDataProperty);
        EditorGUILayout.PropertyField(hitstunDataProperty);
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndScrollView();

        serializedObject.ApplyModifiedProperties();
    }

    public void SetCharacterData(CharacterData data)
    {
        characterData = data;
    }
}
