using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NPCChaseData))]
public class NPCChaseDataEditor : Editor
{
    private NPCChaseData data;
    
    private SerializedProperty CooldownProperty;
    private SerializedProperty DodgeChanceProperty;
    private SerializedProperty DistanceToAttackMultiplierProperty;
    private SerializedProperty DistanceToDodgeProperty;
    private SerializedProperty DistanceToWatchProperty;
    private SerializedProperty DodgeAttemptCooldownProperty;
    
    private NPCChaseData currentTarget;
    private GUIStyle titleStyle;

    private Vector2 scrollPos = Vector2.zero;
    
    private void OnEnable()
    {
        currentTarget = target as NPCChaseData;
    
        CooldownProperty = serializedObject.FindProperty(nameof(NPCChaseData.Cooldown));
        DodgeChanceProperty = serializedObject.FindProperty(nameof(NPCChaseData.DodgeChance));
        DistanceToAttackMultiplierProperty = serializedObject.FindProperty(nameof(NPCChaseData.DistanceToAttackMultiplier));
        DistanceToDodgeProperty = serializedObject.FindProperty(nameof(NPCChaseData.DistanceToDodge));
        DistanceToWatchProperty = serializedObject.FindProperty(nameof(NPCChaseData.DistanceToWatch));
        DodgeAttemptCooldownProperty = serializedObject.FindProperty(nameof(NPCChaseData.DodgeAttemptCooldown));

        titleStyle = new GUIStyle();
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 16;
        titleStyle.normal.textColor = new Color(0.9f, 0.9f, 0.9f);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var boxStyle = new GUIStyle("Box");
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        
        EditorGUILayout.LabelField("Properties", titleStyle);
        EditorGUILayout.Space(5);
        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(CooldownProperty);
        EditorGUILayout.PropertyField(DodgeChanceProperty);
        EditorGUILayout.PropertyField(DodgeAttemptCooldownProperty);
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("State Change Condition", titleStyle);
        EditorGUILayout.Space(5);
        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(DistanceToAttackMultiplierProperty);
        EditorGUILayout.PropertyField(DistanceToWatchProperty);
        EditorGUILayout.PropertyField(DistanceToDodgeProperty);
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
        
        serializedObject.ApplyModifiedProperties();
    }

    public void SetData(NPCChaseData data)
    {
        this.data = data;
    }
}
