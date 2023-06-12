using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NPCHitstunnedData))]
public class NPCHitstunnedDataEditor : Editor
{
    private NPCHitstunnedData data;
    
    private SerializedProperty DodgeChanceProperty;
    private SerializedProperty DistanceToDodgeProperty;
    
    private NPCHitstunnedData currentTarget;
    private GUIStyle titleStyle;

    private Vector2 scrollPos = Vector2.zero;
    
    private void OnEnable()
    {
        currentTarget = target as NPCHitstunnedData;
    
        DodgeChanceProperty = serializedObject.FindProperty(nameof(NPCHitstunnedData.DodgeChance));
        DistanceToDodgeProperty = serializedObject.FindProperty(nameof(NPCHitstunnedData.DistanceToDodge));

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
        EditorGUILayout.PropertyField(DodgeChanceProperty);
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("State Change Condition", titleStyle);
        EditorGUILayout.Space(5);
        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(DistanceToDodgeProperty);
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
        
        serializedObject.ApplyModifiedProperties();
    }

    public void SetData(NPCHitstunnedData data)
    {
        this.data = data;
    }
}
