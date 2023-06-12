using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NPCDodgingData))]
public class NPCDodgingDataEditor : Editor
{
    private NPCDodgingData data;
    
    private SerializedProperty CooldownProperty;
    private SerializedProperty EnemyDistanceToDodgeProperty;
    
    private NPCDodgingData currentTarget;
    private GUIStyle titleStyle;

    private Vector2 scrollPos = Vector2.zero;
    
    private void OnEnable()
    {
        currentTarget = target as NPCDodgingData;
    
        CooldownProperty = serializedObject.FindProperty(nameof(NPCDodgingData.Cooldown));
        EnemyDistanceToDodgeProperty = serializedObject.FindProperty(nameof(NPCDodgingData.EnemyDistanceToDodge));

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
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("State Change Condition", titleStyle);
        EditorGUILayout.Space(5);
        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(EnemyDistanceToDodgeProperty);
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
        
        serializedObject.ApplyModifiedProperties();
    }

    public void SetData(NPCDodgingData data)
    {
        this.data = data;
    }
}
