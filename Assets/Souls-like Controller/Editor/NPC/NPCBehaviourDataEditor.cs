using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AttackComboData))]
public class NPCBehaviourDataEditor : Editor
{
    private NPCBehaviourData data;
    
    private SerializedProperty BehaviourProperty;
    private SerializedProperty MinAggresionFactorProperty;
    private SerializedProperty MaxAggresionFactorProperty;
    private SerializedProperty AttackingStateProperty;
    private SerializedProperty ChaseStateProperty;
    private SerializedProperty HitstunnedStateProperty;
    private SerializedProperty WanderingStateProperty;
    private SerializedProperty WatchingStateProperty;
    private SerializedProperty DodgingStateProperty;
    
    private NPCBehaviourData currentTarget;
    private GUIStyle titleStyle;

    private Vector2 scrollPos = Vector2.zero;
    
    private void OnEnable()
    {
        currentTarget = target as NPCBehaviourData;
    
        BehaviourProperty = serializedObject.FindProperty(nameof(NPCBehaviourData.Behaviour));
        MinAggresionFactorProperty = serializedObject.FindProperty(nameof(NPCBehaviourData.MinAggresionFactor));
        MaxAggresionFactorProperty = serializedObject.FindProperty(nameof(NPCBehaviourData.MaxAggresionFactor));
        AttackingStateProperty = serializedObject.FindProperty(nameof(NPCBehaviourData.AttackingState));
        ChaseStateProperty = serializedObject.FindProperty(nameof(NPCBehaviourData.ChaseState));
        HitstunnedStateProperty = serializedObject.FindProperty(nameof(NPCBehaviourData.HitstunnedState));
        WanderingStateProperty = serializedObject.FindProperty(nameof(NPCBehaviourData.WanderingState));
        WatchingStateProperty = serializedObject.FindProperty(nameof(NPCBehaviourData.WatchingState));
        DodgingStateProperty = serializedObject.FindProperty(nameof(NPCBehaviourData.DodgingState));

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
        
        EditorGUILayout.LabelField("Aggression", titleStyle);
        EditorGUILayout.Space(5);
        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(BehaviourProperty);
        EditorGUILayout.PropertyField(MinAggresionFactorProperty);
        EditorGUILayout.PropertyField(MaxAggresionFactorProperty);
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("States", titleStyle);
        EditorGUILayout.Space(5);
        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(WanderingStateProperty);
        EditorGUILayout.PropertyField(WatchingStateProperty);
        EditorGUILayout.PropertyField(ChaseStateProperty);
        EditorGUILayout.PropertyField(AttackingStateProperty);
        EditorGUILayout.PropertyField(DodgingStateProperty);
        EditorGUILayout.PropertyField(HitstunnedStateProperty);
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
        
        serializedObject.ApplyModifiedProperties();
    }

    public void SetData(NPCBehaviourData data)
    {
        this.data = data;
    }
}
