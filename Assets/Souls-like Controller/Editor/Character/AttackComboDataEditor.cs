using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AttackComboData))]
public class AttackComboDataEditor : Editor
{
    private AttackComboData comboData;
    
    private SerializedProperty attacksListProperty;
    
    private AttackComboData currentTarget;
    private GUIStyle titleStyle;

    private Vector2 scrollPos = Vector2.zero;
    
    private void OnEnable()
    {
        currentTarget = target as AttackComboData;
    
        attacksListProperty = serializedObject.FindProperty(nameof(AttackComboData.AttacksList));

        titleStyle = new GUIStyle();
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 16;
        titleStyle.normal.textColor = new Color(0.9f, 0.9f, 0.9f);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        
        EditorGUILayout.LabelField("Attacks In Combo", titleStyle);
        EditorGUILayout.Space(5);
        var boxStyle = new GUIStyle("Box");
        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.PropertyField(attacksListProperty);
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
        
        serializedObject.ApplyModifiedProperties();
    }

    public void SetComboData(AttackComboData data)
    {
        comboData = data;
    }
}
