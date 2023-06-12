using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EquipmentData))]
public class EquipmentDataEditor : Editor
{
    private EquipmentData equipmentData;
    
    private SerializedProperty primaryActionOnRightHandProperty;
    private SerializedProperty secondaryActionOnRightHandProperty;
    private SerializedProperty primaryActionOnLeftHandProperty;
    private SerializedProperty secondaryActionOnLeftHandProperty;
    
    private SerializedProperty baseAttackProperty;
    private SerializedProperty attackStrengthScalingProperty;
    private SerializedProperty poiseDamageProperty;
    private SerializedProperty lightAttackComboProperty;
    private SerializedProperty heavyAttackComboProperty;

    private SerializedProperty blockBaseProperty;
    private SerializedProperty blockPoiseProperty;
    private SerializedProperty blockingDataProperty;

    private SerializedProperty showHitboxesProperty;
    
    private EquipmentData currentTarget;
    private GUIStyle titleStyle;
    
    private Vector2 scrollPos = Vector2.zero;

    private void OnEnable()
    {
        currentTarget = target as EquipmentData;
    
        primaryActionOnRightHandProperty = serializedObject.FindProperty(nameof(EquipmentData.PrimaryRightHand));
        secondaryActionOnRightHandProperty = serializedObject.FindProperty(nameof(EquipmentData.SecondaryRightHand));
        primaryActionOnLeftHandProperty = serializedObject.FindProperty(nameof(EquipmentData.PrimaryLeftHand));
        secondaryActionOnLeftHandProperty = serializedObject.FindProperty(nameof(EquipmentData.SecondaryLeftHand));
        
        baseAttackProperty = serializedObject.FindProperty(nameof(EquipmentData.BaseAttack));
        attackStrengthScalingProperty = serializedObject.FindProperty(nameof(EquipmentData.AttackStrengthScaling));
        poiseDamageProperty = serializedObject.FindProperty(nameof(EquipmentData.PoiseDamage));
        lightAttackComboProperty = serializedObject.FindProperty(nameof(EquipmentData.PrimaryAttackCombo));
        heavyAttackComboProperty = serializedObject.FindProperty(nameof(EquipmentData.SecondaryAttackCombo));
        
        blockBaseProperty = serializedObject.FindProperty(nameof(EquipmentData.BlockDamageReduction));
        blockPoiseProperty = serializedObject.FindProperty(nameof(EquipmentData.BlockPoise));
        blockingDataProperty = serializedObject.FindProperty(nameof(EquipmentData.BlockingData));
        
        showHitboxesProperty = serializedObject.FindProperty(nameof(EquipmentData.ShowHitboxes));
        
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

        EditorGUILayout.LabelField("Possible actions", titleStyle);
        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(primaryActionOnRightHandProperty);
        EditorGUILayout.PropertyField(secondaryActionOnRightHandProperty);
        EditorGUILayout.PropertyField(primaryActionOnLeftHandProperty);
        EditorGUILayout.PropertyField(secondaryActionOnLeftHandProperty);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("Attack Data", titleStyle);
        EditorGUILayout.Space(5);

        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(baseAttackProperty);
        EditorGUILayout.PropertyField(attackStrengthScalingProperty);
        EditorGUILayout.PropertyField(poiseDamageProperty);
        EditorGUILayout.PropertyField(lightAttackComboProperty);
        EditorGUILayout.PropertyField(heavyAttackComboProperty);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("Block Data", titleStyle);
        EditorGUILayout.Space(5);

        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(blockBaseProperty);
        EditorGUILayout.PropertyField(blockPoiseProperty);
        EditorGUILayout.PropertyField(blockingDataProperty);
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("Debug", titleStyle);
        EditorGUILayout.Space(5);

        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(showHitboxesProperty);
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndScrollView();

        serializedObject.ApplyModifiedProperties();
    }

    public void SetEquipmentData(EquipmentData data)
    {
        equipmentData = data;
    }
}
