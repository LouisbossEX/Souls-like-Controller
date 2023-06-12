using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NPCAttackingData))]
public class NPCAttackingDataEditor : Editor
{
    private NPCAttackingData data;
    
    private SerializedProperty CooldownProperty;
    
    private NPCAttackingData currentTarget;
    private GUIStyle titleStyle;

    private Vector2 scrollPos = Vector2.zero;
    
    private void OnEnable()
    {
        currentTarget = target as NPCAttackingData;
    
        CooldownProperty = serializedObject.FindProperty(nameof(NPCAttackingData.Cooldown));
        
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

        EditorGUILayout.EndScrollView();
        
        serializedObject.ApplyModifiedProperties();
    }

    public void SetData(NPCAttackingData data)
    {
        this.data = data;
    }
}
