using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NPCWanderingData))]
public class NPCWanderingDataEditor : Editor
{
    private NPCWanderingData data;
    
    private SerializedProperty WanderMaxLengthProperty;
    private SerializedProperty EnemyDetectionRangeProperty;
    
    private NPCWanderingData currentTarget;
    private GUIStyle titleStyle;

    private Vector2 scrollPos = Vector2.zero;
    
    private void OnEnable()
    {
        currentTarget = target as NPCWanderingData;
    
        WanderMaxLengthProperty = serializedObject.FindProperty(nameof(NPCWanderingData.WanderMaxTime));
        EnemyDetectionRangeProperty = serializedObject.FindProperty(nameof(NPCWanderingData.EnemyDetectionRange));

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
        EditorGUILayout.PropertyField(WanderMaxLengthProperty);
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("State Change Condition", titleStyle);
        EditorGUILayout.Space(5);
        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(EnemyDetectionRangeProperty);
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
        
        serializedObject.ApplyModifiedProperties();
    }

    public void SetData(NPCWanderingData data)
    {
        this.data = data;
    }
}
