using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NPCWatchingData))]
public class NPCWatchingDataEditor : Editor
{
    private NPCWatchingData data;
    
    private SerializedProperty CooldownProperty;
    private SerializedProperty EnemyDistanceToChaseProperty;
    private SerializedProperty ReenterStateChanceProperty;
    private SerializedProperty WatchingMinTimeProperty;
    private SerializedProperty WatchingMaxTimeProperty;
    
    private NPCWatchingData currentTarget;
    private GUIStyle titleStyle;

    private Vector2 scrollPos = Vector2.zero;
    
    private void OnEnable()
    {
        currentTarget = target as NPCWatchingData;
    
        CooldownProperty = serializedObject.FindProperty(nameof(NPCWatchingData.Cooldown));
        EnemyDistanceToChaseProperty = serializedObject.FindProperty(nameof(NPCWatchingData.EnemyDistanceToChase));
        ReenterStateChanceProperty = serializedObject.FindProperty(nameof(NPCWatchingData.ReenterStateChance));
        WatchingMinTimeProperty = serializedObject.FindProperty(nameof(NPCWatchingData.WatchingMinTime));
        WatchingMaxTimeProperty = serializedObject.FindProperty(nameof(NPCWatchingData.WatchingMaxTime));

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
        EditorGUILayout.PropertyField(WatchingMinTimeProperty);
        EditorGUILayout.PropertyField(WatchingMaxTimeProperty);
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("State Change Condition", titleStyle);
        EditorGUILayout.Space(5);
        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(ReenterStateChanceProperty);
        EditorGUILayout.PropertyField(EnemyDistanceToChaseProperty);
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
        
        serializedObject.ApplyModifiedProperties();
    }

    public void SetData(NPCWatchingData data)
    {
        this.data = data;
    }
}
