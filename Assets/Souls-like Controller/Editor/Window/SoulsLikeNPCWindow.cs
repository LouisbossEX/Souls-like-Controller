using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoulsLikeNPCWindow : EditorWindow
{
    private Editor behaviourDataEditor;
    private Editor attackingDataEditor;
    private Editor chaseDataEditor;
    private Editor dodgingDataEditor;
    private Editor hitstunnedDataEditor;
    private Editor wanderingDataEditor;
    private Editor watchingDataEditor;

    private NPCBehaviourData behaviourData;
    private NPCAttackingData attackingData;
    private NPCChaseData chaseData;
    private NPCDodgingData dodgingData;
    private NPCHitstunnedData hitstunnedData;
    private NPCWanderingData wanderingData;
    private NPCWatchingData watchingData;

    private string rootFilePath = "Assets/";
    private string additionalFilePath = "Data/NPC Behaviours";
    
    private enum ECharacterPages : int
    {
        Behaviour = 0,
        Wandering,
        Watching,
        Chasing,
        Attacking,
        Dodging,
        Hitstunned,
        Max
    }
    
    private static readonly List<string> characterPageNames = new List<string>
    {
        "Behaviour",
        "Wandering",
        "Watching",
        "Chasing",
        "Attacking",
        "Dodging",
        "Hitstunned",
    };
    
    private GUIStyle crumbStyle;
    
    private ECharacterPages currentPage = ECharacterPages.Behaviour;
    
    private Color buttonDefault;
    private Color buttonHighlight;
    private GUIStyle buttonTitleStyle;
    private GUIStyle titleStyle;
    
    private string newObjectName;

    private Vector2 scrollPos = Vector2.zero;

    [MenuItem("Souls/NPC Settings")]
    public static void ShowWindow()
    {
        var instance = GetWindow<SoulsLikeNPCWindow>();
        instance.ShowAuxWindow();
    }
    
    private void OnEnable()
    {
        buttonTitleStyle = new GUIStyle();
        buttonTitleStyle.normal.textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black;
        buttonTitleStyle.wordWrap = true;
        buttonTitleStyle.fontStyle = FontStyle.Bold;
        buttonTitleStyle.alignment = TextAnchor.MiddleLeft;

        crumbStyle = new GUIStyle(buttonTitleStyle);
        buttonDefault = EditorGUIUtility.isProSkin ? Color.gray : Color.gray;
        buttonHighlight = EditorGUIUtility.isProSkin ? Color.white : Color.black;
        
        titleStyle = new GUIStyle();
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 16;
        titleStyle.normal.textColor = new Color(0.9f, 0.9f, 0.9f);
    }

    private void OnGUI()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            OutsideLayout();
            
            using (new EditorGUILayout.VerticalScope("box", GUILayout.ExpandHeight(true)))
            {
                switch (currentPage)
                {
                    case ECharacterPages.Attacking: AttackingPage(); break;
                    case ECharacterPages.Chasing: ChasePage(); break;
                    case ECharacterPages.Dodging: DodgePage(); break;
                    case ECharacterPages.Behaviour: BehaviourPage(); break;
                    case ECharacterPages.Hitstunned: HitstunnedPage(); break;
                    case ECharacterPages.Wandering: WanderingPage(); break;
                    case ECharacterPages.Watching: WatchingPage(); break;
                }
            }
        }
    }
    
    private void OutsideLayout()
    {
        using (new EditorGUILayout.VerticalScope("box"))
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(160));
            
            crumbStyle.alignment = TextAnchor.MiddleCenter;
            Color oldColor = GUI.backgroundColor;

            for (int i = 0; i < characterPageNames.Count; i++)
            {
                GUI.backgroundColor = i == (int)currentPage ? Color.white : Color.gray;
                crumbStyle.normal.textColor = (i == (int)currentPage ? buttonHighlight : buttonDefault);
                if (i == 0)
                {
                    EditorGUILayout.LabelField("Behaviour", titleStyle, GUILayout.Width(150));
                    EditorGUILayout.Space(5f);
                }
                if (i == 1)
                {
                    EditorGUILayout.Space(15f);
                    EditorGUILayout.LabelField("States", titleStyle, GUILayout.Width(150));
                    EditorGUILayout.Space(5f);
                }
                using (var b = new EditorGUILayout.HorizontalScope("button", GUILayout.Height(40)))
                {
                    if (GUI.Button(b.rect, characterPageNames[i], crumbStyle))
                    {
                        currentPage = (ECharacterPages)i;
                    }
                    EditorGUILayout.Space();
                }
                GUI.backgroundColor = oldColor;
            }
            
            EditorGUILayout.EndScrollView();
            
            GUILayout.FlexibleSpace();
        }
    }
    
    private void BehaviourPage()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("New Data Name: ", GUILayout.Width(100));
            newObjectName = EditorGUILayout.TextField(newObjectName);
            var isGUIEnabled = GUI.enabled;
            GUI.enabled = !string.IsNullOrEmpty(newObjectName);
            if (GUILayout.Button("Create New Behaviour Data",GUILayout.Width(200)))
            {
                var allDatas = AssetDatabase.FindAssets("t: " + nameof(NPCBehaviourData))
                    .Select(x => AssetDatabase.LoadAssetAtPath<NPCBehaviourData>(AssetDatabase.GUIDToAssetPath(x)))
                    .ToList();

                bool objectExists = false;
                
                foreach (var item in allDatas)
                {
                    if (item.name == newObjectName)
                    {
                        objectExists = true;
                    }
                }

                if (!objectExists)
                {
                    NPCBehaviourData newScriptable = CreateInstance<NPCBehaviourData>();
                    string newAdditionalPath = additionalFilePath + "/" + nameof(NPCBehaviourData);
                    var folderPath = Path.Combine(Application.dataPath, newAdditionalPath);
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    var scriptableObjectName = Path.Combine(rootFilePath + newAdditionalPath, newObjectName) + ".asset";
                    AssetDatabase.CreateAsset(newScriptable, scriptableObjectName);
                    behaviourData = newScriptable;
                }
            }
            GUI.enabled = isGUIEnabled;
        }
        
        EditorGUILayout.Space(20);
        
        EditorGUILayout.LabelField("Selected NPC Behaviour Data", titleStyle);
        EditorGUILayout.Space(5);
        behaviourData = EditorGUILayout.ObjectField(behaviourData, typeof(NPCBehaviourData), true) as NPCBehaviourData;
        
        EditorGUILayout.Space(20);
        
        if (behaviourData != null)
        {
            Editor.CreateCachedEditor(behaviourData, typeof(NPCBehaviourDataEditor), ref behaviourDataEditor);
            ((NPCBehaviourDataEditor)behaviourDataEditor).SetData(behaviourData);
            behaviourDataEditor.OnInspectorGUI();
            
            if(behaviourData.AttackingState != null)
                attackingData = behaviourData.AttackingState;
            if(behaviourData.ChaseState != null)
                chaseData = behaviourData.ChaseState;
            if(behaviourData.DodgingState != null)
                dodgingData = behaviourData.DodgingState;
            if(behaviourData.HitstunnedState != null)
                hitstunnedData = behaviourData.HitstunnedState;
            if(behaviourData.WanderingState != null)
                wanderingData = behaviourData.WanderingState;
            if(behaviourData.WatchingState != null)
                watchingData = behaviourData.WatchingState;

        }
    }
    
    private void WanderingPage()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("New Data Name: ", GUILayout.Width(100));
            newObjectName = EditorGUILayout.TextField(newObjectName);
            var isGUIEnabled = GUI.enabled;
            GUI.enabled = !string.IsNullOrEmpty(newObjectName);
            if (GUILayout.Button("Create New Behaviour Data",GUILayout.Width(200)))
            {
                var allDatas = AssetDatabase.FindAssets("t: " + nameof(NPCWanderingData))
                    .Select(x => AssetDatabase.LoadAssetAtPath<NPCWanderingData>(AssetDatabase.GUIDToAssetPath(x)))
                    .ToList();

                bool objectExists = false;
                
                foreach (var item in allDatas)
                {
                    if (item.name == newObjectName)
                    {
                        objectExists = true;
                    }
                }

                if (!objectExists)
                {
                    NPCWanderingData newScriptable = CreateInstance<NPCWanderingData>();
                    string newAdditionalPath = additionalFilePath + "/" + nameof(NPCWanderingData);
                    var folderPath = Path.Combine(Application.dataPath, newAdditionalPath);
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    var scriptableObjectName = Path.Combine(rootFilePath + newAdditionalPath, newObjectName) + ".asset";
                    AssetDatabase.CreateAsset(newScriptable, scriptableObjectName);
                    wanderingData = newScriptable;
                }
            }
            GUI.enabled = isGUIEnabled;
        }
        
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Selected Wandering Data", titleStyle);
        EditorGUILayout.Space(5);
        wanderingData = EditorGUILayout.ObjectField(wanderingData, typeof(NPCWanderingData), true) as NPCWanderingData;
        
        EditorGUILayout.Space(20);

        if (wanderingData != null)
        {
            Editor.CreateCachedEditor(wanderingData, typeof(NPCWanderingDataEditor), ref wanderingDataEditor);
            ((NPCWanderingDataEditor)wanderingDataEditor).SetData(wanderingData);
            wanderingDataEditor.OnInspectorGUI();
        }
    }
    
    private void WatchingPage()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("New Data Name: ", GUILayout.Width(100));
            newObjectName = EditorGUILayout.TextField(newObjectName);
            var isGUIEnabled = GUI.enabled;
            GUI.enabled = !string.IsNullOrEmpty(newObjectName);
            if (GUILayout.Button("Create New Watching Data",GUILayout.Width(200)))
            {
                var allDatas = AssetDatabase.FindAssets("t: " + nameof(NPCWatchingData))
                    .Select(x => AssetDatabase.LoadAssetAtPath<NPCWatchingData>(AssetDatabase.GUIDToAssetPath(x)))
                    .ToList();

                bool objectExists = false;
                
                foreach (var item in allDatas)
                {
                    if (item.name == newObjectName)
                    {
                        objectExists = true;
                    }
                }

                if (!objectExists)
                {
                    NPCWatchingData newScriptable = CreateInstance<NPCWatchingData>();
                    string newAdditionalPath = additionalFilePath + "/" + nameof(NPCWatchingData);
                    var folderPath = Path.Combine(Application.dataPath, newAdditionalPath);
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    var scriptableObjectName = Path.Combine(rootFilePath + newAdditionalPath, newObjectName) + ".asset";
                    AssetDatabase.CreateAsset(newScriptable, scriptableObjectName);
                    watchingData = newScriptable;
                }
            }
            GUI.enabled = isGUIEnabled;
        }
        
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Selected Watching Data", titleStyle);
        EditorGUILayout.Space(5);
        watchingData = EditorGUILayout.ObjectField(watchingData, typeof(NPCWatchingData), true) as NPCWatchingData;
        
        EditorGUILayout.Space(20);

        if (watchingData != null)
        {
            Editor.CreateCachedEditor(watchingData, typeof(NPCWatchingDataEditor), ref watchingDataEditor);
            ((NPCWatchingDataEditor)watchingDataEditor).SetData(watchingData);
            watchingDataEditor.OnInspectorGUI();
        }
    }
    
    private void ChasePage()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("New Data Name: ", GUILayout.Width(100));
            newObjectName = EditorGUILayout.TextField(newObjectName);
            var isGUIEnabled = GUI.enabled;
            GUI.enabled = !string.IsNullOrEmpty(newObjectName);
            if (GUILayout.Button("Create New Chasing Data",GUILayout.Width(200)))
            {
                var allDatas = AssetDatabase.FindAssets("t: " + nameof(NPCChaseData))
                    .Select(x => AssetDatabase.LoadAssetAtPath<NPCChaseData>(AssetDatabase.GUIDToAssetPath(x)))
                    .ToList();

                bool objectExists = false;
                
                foreach (var item in allDatas)
                {
                    if (item.name == newObjectName)
                    {
                        objectExists = true;
                    }
                }

                if (!objectExists)
                {
                    NPCChaseData newScriptable = CreateInstance<NPCChaseData>();
                    string newAdditionalPath = additionalFilePath + "/" + nameof(NPCChaseData);
                    var folderPath = Path.Combine(Application.dataPath, newAdditionalPath);
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    var scriptableObjectName = Path.Combine(rootFilePath + newAdditionalPath, newObjectName) + ".asset";
                    AssetDatabase.CreateAsset(newScriptable, scriptableObjectName);
                    chaseData = newScriptable;
                }
            }
            GUI.enabled = isGUIEnabled;
        }
        
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Selected Chasing Data", titleStyle);
        EditorGUILayout.Space(5);
        chaseData = EditorGUILayout.ObjectField(chaseData, typeof(NPCChaseData), true) as NPCChaseData;
        
        EditorGUILayout.Space(20);

        if (chaseData != null)
        {
            Editor.CreateCachedEditor(chaseData, typeof(NPCChaseDataEditor), ref chaseDataEditor);
            ((NPCChaseDataEditor)chaseDataEditor).SetData(chaseData);
            chaseDataEditor.OnInspectorGUI();
        }
    }
    
    private void DodgePage()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("New Data Name: ", GUILayout.Width(100));
            newObjectName = EditorGUILayout.TextField(newObjectName);
            var isGUIEnabled = GUI.enabled;
            GUI.enabled = !string.IsNullOrEmpty(newObjectName);
            if (GUILayout.Button("Create New Dodging Data",GUILayout.Width(200)))
            {
                var allDatas = AssetDatabase.FindAssets("t: " + nameof(NPCDodgingData))
                    .Select(x => AssetDatabase.LoadAssetAtPath<NPCDodgingData>(AssetDatabase.GUIDToAssetPath(x)))
                    .ToList();

                bool objectExists = false;
                
                foreach (var item in allDatas)
                {
                    if (item.name == newObjectName)
                    {
                        objectExists = true;
                    }
                }

                if (!objectExists)
                {
                    NPCDodgingData newScriptable = CreateInstance<NPCDodgingData>();
                    string newAdditionalPath = additionalFilePath + "/" + nameof(NPCDodgingData);
                    var folderPath = Path.Combine(Application.dataPath, newAdditionalPath);
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    var scriptableObjectName = Path.Combine(rootFilePath + newAdditionalPath, newObjectName) + ".asset";
                    AssetDatabase.CreateAsset(newScriptable, scriptableObjectName);
                    dodgingData = newScriptable;
                }
            }
            GUI.enabled = isGUIEnabled;
        }
        
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Selected Dodging Data", titleStyle);
        EditorGUILayout.Space(5);
        dodgingData = EditorGUILayout.ObjectField(dodgingData, typeof(NPCDodgingData), true) as NPCDodgingData;
        
        EditorGUILayout.Space(20);

        if (dodgingData != null)
        {
            Editor.CreateCachedEditor(dodgingData, typeof(NPCDodgingDataEditor), ref dodgingDataEditor);
            ((NPCDodgingDataEditor)dodgingDataEditor).SetData(dodgingData);
            dodgingDataEditor.OnInspectorGUI();
        }
    }
    
    private void AttackingPage()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("New Data Name: ", GUILayout.Width(100));
            newObjectName = EditorGUILayout.TextField(newObjectName);
            var isGUIEnabled = GUI.enabled;
            GUI.enabled = !string.IsNullOrEmpty(newObjectName);
            if (GUILayout.Button("Create New Attacking Data",GUILayout.Width(200)))
            {
                var allDatas = AssetDatabase.FindAssets("t: " + nameof(NPCAttackingData))
                    .Select(x => AssetDatabase.LoadAssetAtPath<NPCAttackingData>(AssetDatabase.GUIDToAssetPath(x)))
                    .ToList();

                bool objectExists = false;
                
                foreach (var item in allDatas)
                {
                    if (item.name == newObjectName)
                    {
                        objectExists = true;
                    }
                }

                if (!objectExists)
                {
                    NPCAttackingData newScriptable = CreateInstance<NPCAttackingData>();
                    string newAdditionalPath = additionalFilePath + "/" + nameof(NPCAttackingData);
                    var folderPath = Path.Combine(Application.dataPath, newAdditionalPath);
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    var scriptableObjectName = Path.Combine(rootFilePath + newAdditionalPath, newObjectName) + ".asset";
                    AssetDatabase.CreateAsset(newScriptable, scriptableObjectName);
                    attackingData = newScriptable;
                }
            }
            GUI.enabled = isGUIEnabled;
        }
        
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Selected Attacking Data", titleStyle);
        EditorGUILayout.Space(5);
        attackingData = EditorGUILayout.ObjectField(attackingData, typeof(NPCAttackingData), true) as NPCAttackingData;
        
        EditorGUILayout.Space(20);

        if (attackingData != null)
        {
            Editor.CreateCachedEditor(attackingData, typeof(NPCAttackingDataEditor), ref attackingDataEditor);
            ((NPCAttackingDataEditor)attackingDataEditor).SetData(attackingData);
            attackingDataEditor.OnInspectorGUI();
        }
    }
    
    private void HitstunnedPage()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("New Data Name: ", GUILayout.Width(100));
            newObjectName = EditorGUILayout.TextField(newObjectName);
            var isGUIEnabled = GUI.enabled;
            GUI.enabled = !string.IsNullOrEmpty(newObjectName);
            if (GUILayout.Button("Create New Hitstunned Data",GUILayout.Width(200)))
            {
                var allDatas = AssetDatabase.FindAssets("t: " + nameof(NPCHitstunnedData))
                    .Select(x => AssetDatabase.LoadAssetAtPath<NPCHitstunnedData>(AssetDatabase.GUIDToAssetPath(x)))
                    .ToList();

                bool objectExists = false;
                
                foreach (var item in allDatas)
                {
                    if (item.name == newObjectName)
                    {
                        objectExists = true;
                    }
                }

                if (!objectExists)
                {
                    NPCHitstunnedData newScriptable = CreateInstance<NPCHitstunnedData>();
                    string newAdditionalPath = additionalFilePath + "/" + nameof(NPCHitstunnedData);
                    var folderPath = Path.Combine(Application.dataPath, newAdditionalPath);
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    var scriptableObjectName = Path.Combine(rootFilePath + newAdditionalPath, newObjectName) + ".asset";
                    AssetDatabase.CreateAsset(newScriptable, scriptableObjectName);
                    hitstunnedData = newScriptable;
                }
            }
            GUI.enabled = isGUIEnabled;
        }
        
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Selected Hitstunned Data", titleStyle);
        EditorGUILayout.Space(5);
        hitstunnedData = EditorGUILayout.ObjectField(hitstunnedData, typeof(NPCHitstunnedData), true) as NPCHitstunnedData;
        
        EditorGUILayout.Space(20);

        if (hitstunnedData != null)
        {
            Editor.CreateCachedEditor(hitstunnedData, typeof(NPCHitstunnedDataEditor), ref hitstunnedDataEditor);
            ((NPCHitstunnedDataEditor)hitstunnedDataEditor).SetData(hitstunnedData);
            hitstunnedDataEditor.OnInspectorGUI();
        }
    }
}
