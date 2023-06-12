using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BlockingData))]
public class BlockingDataEditor : Editor
{
    private BlockingData blockingData;
    
    private SerializedProperty movementReduction;
    private SerializedProperty timeToStartBlocking;
    private SerializedProperty animationIDProperty;
    
    private BlockingData currentTarget;
    private GUIStyle titleStyle;
    private Vector2 scrollPos = Vector2.zero;
    private void OnEnable()
    {
        currentTarget = target as BlockingData;
    
        movementReduction = serializedObject.FindProperty(nameof(BlockingData.MovementSpeedMultiplier));
        timeToStartBlocking = serializedObject.FindProperty(nameof(BlockingData.TimeToStartBlocking));
        animationIDProperty = serializedObject.FindProperty(nameof(BlockingData.AnimationID));

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
        EditorGUILayout.LabelField("Animation", titleStyle);
        EditorGUILayout.Space(5);

        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(animationIDProperty);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("Movement", titleStyle);
        EditorGUILayout.Space(5);

        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(movementReduction);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("Block", titleStyle);
        EditorGUILayout.Space(5);

        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(timeToStartBlocking);
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndScrollView();

        serializedObject.ApplyModifiedProperties();
    }

    public void SetBlockData(BlockingData data)
    {
        blockingData = data;
    }
}
