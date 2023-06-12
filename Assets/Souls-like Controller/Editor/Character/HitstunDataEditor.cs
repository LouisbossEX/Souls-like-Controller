using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HitstunData))]
public class HitstunDataEditor : Editor
{
    private HitstunData hitstunData;

    private Rect rect;

    //State
    private SerializedProperty timeBeforeCanTransitionProperty;
    private SerializedProperty durationProperty;
    private SerializedProperty canTransitionStatesProperty;
    private SerializedProperty canAlwaysTransitionStatesProperty;
    private SerializedProperty animationIDProperty;

    private HitstunData currentTarget;
    private GUIStyle titleStyle;

    private Vector2 scrollPos = Vector2.zero;
    
    private void OnEnable()
    {
        currentTarget = target as HitstunData;

        //State
        timeBeforeCanTransitionProperty = serializedObject.FindProperty(nameof(HitstunData.TimeBeforeCanTransition));
        durationProperty = serializedObject.FindProperty(nameof(HitstunData.Duration));
        canTransitionStatesProperty = serializedObject.FindProperty(nameof(HitstunData.CanTransitionStates));
        canAlwaysTransitionStatesProperty = serializedObject.FindProperty(nameof(HitstunData.CanAlwaysTransitionStates));
        animationIDProperty = serializedObject.FindProperty(nameof(HitstunData.AnimationID));
        
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
        EditorGUILayout.PropertyField(timeBeforeCanTransitionProperty);
        EditorGUILayout.PropertyField(canTransitionStatesProperty);
        EditorGUILayout.PropertyField(canAlwaysTransitionStatesProperty);
        EditorGUILayout.PropertyField(durationProperty);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("Animation", titleStyle);
        EditorGUILayout.Space(5);

        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(animationIDProperty);
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();

        serializedObject.ApplyModifiedProperties();
    }

    public void SetHitstunData(HitstunData data)
    {
        hitstunData = data;
    }
}
