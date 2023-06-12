using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PhysicsData))]
public class PhysicsDataEditor : Editor
{
    private PhysicsData physicsData;
    
    private SerializedProperty impulseVelocityDecaySpeedProperty;
    private SerializedProperty gravityProperty;

    private PhysicsData currentTarget;
    private GUIStyle titleStyle;

    private Vector2 scrollPos = Vector2.zero;
    
    private void OnEnable()
    {
        currentTarget = target as PhysicsData;
    
        impulseVelocityDecaySpeedProperty = serializedObject.FindProperty(nameof(PhysicsData.ImpulseVelocityDecaySpeed));
        gravityProperty = serializedObject.FindProperty(nameof(PhysicsData.Gravity));
        
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

        EditorGUILayout.LabelField("Transition To Other States", titleStyle);
        EditorGUILayout.Space(5);

        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(impulseVelocityDecaySpeedProperty);
        EditorGUILayout.PropertyField(gravityProperty);
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndScrollView();
        
        serializedObject.ApplyModifiedProperties();
    }

    public void SetPhysicsData(PhysicsData data)
    {
        physicsData = data;
    }
}
