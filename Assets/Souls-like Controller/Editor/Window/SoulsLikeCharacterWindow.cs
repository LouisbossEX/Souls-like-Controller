using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoulsLikeCharacterWindow : EditorWindow
{
    private Editor characterDataEditor;
    private Editor attackDataEditor;
    private Editor blockingDataEditor;
    private Editor comboDataEditor;
    private Editor dodgeDataEditor;
    private Editor equipmentDataEditor;
    private Editor movementDataEditor;
    private Editor physicsDataEditor;
    private Editor hitstunDataEditor;
    
    private CharacterData characterData;
    private MovementData movementData;
    private DodgeData dodgeData;
    private PhysicsData physicsData;
    private EquipmentData equipmentData;
    private AttackComboData comboData;
    private AttackData attackData;
    private BlockingData blockingData;
    private HitstunData hitstunData;

    private string rootFilePath = "Assets/";
    private string additionalFilePath = "Data/Characters";
    
    private enum ECharacterPages : int
    {
        CharacterData = 0,
        MovementData,
        DodgeData,
        HitstunData,
        PhysicsData,
        EquipmentData,
        ComboData,
        AttackData,
        BlockData,
        Max
    }
    
    private static readonly List<string> characterPageNames = new List<string>
    {
        "Character",
        "Movement",
        "Dodge",
        "Hitstun",
        "Physics",
        "Equipment",
        "Attack Combo",
        "Attack",
        "Block",
    };
    
    private GUIStyle crumbStyle;
    
    private ECharacterPages currentPage = ECharacterPages.CharacterData;
    
    private Color buttonDefault;
    private Color buttonHighlight;
    private GUIStyle buttonTitleStyle;
    private GUIStyle titleStyle;
    
    private string newObjectName;

    private Vector2 scrollPos = Vector2.zero;

    [MenuItem("Souls/Character Settings")]
    public static void ShowWindow()
    {
        var instance = GetWindow<SoulsLikeCharacterWindow>();
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
                    case ECharacterPages.CharacterData: CharacterPage(); break;
                    case ECharacterPages.AttackData: AttackPage(); break;
                    case ECharacterPages.BlockData: BlockPage(); break;
                    case ECharacterPages.ComboData: ComboPage(); break;
                    case ECharacterPages.DodgeData: DodgePage(); break;
                    case ECharacterPages.EquipmentData: EquipmentPage(); break;
                    case ECharacterPages.MovementData: MovementPage(); break;
                    case ECharacterPages.PhysicsData: PhysicsPage(); break;
                    case ECharacterPages.HitstunData: HitstunPage(); break;
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
                    EditorGUILayout.LabelField("Character", titleStyle, GUILayout.Width(150));
                    EditorGUILayout.Space(5f);
                }
                if (i == 5)
                {
                    EditorGUILayout.Space(15f);
                    EditorGUILayout.LabelField("Equipment", titleStyle, GUILayout.Width(150));
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
    
    private void CharacterPage()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("New Data Name: ", GUILayout.Width(100));
            newObjectName = EditorGUILayout.TextField(newObjectName);
            var isGUIEnabled = GUI.enabled;
            GUI.enabled = !string.IsNullOrEmpty(newObjectName);
            if (GUILayout.Button("Create New Character Data",GUILayout.Width(200)))
            {
                var allCharacterData = AssetDatabase.FindAssets("t: " + nameof(CharacterData))
                    .Select(x => AssetDatabase.LoadAssetAtPath<CharacterData>(AssetDatabase.GUIDToAssetPath(x)))
                    .ToList();

                bool objectExists = false;
                
                foreach (var item in allCharacterData)
                {
                    if (item.name == newObjectName)
                    {
                        objectExists = true;
                    }
                }

                if (!objectExists)
                {
                    CharacterData newScriptable = CreateInstance<CharacterData>();
                    string newAdditionalPath = additionalFilePath + "/" + nameof(CharacterData);
                    var folderPath = Path.Combine(Application.dataPath, newAdditionalPath);
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    var scriptableObjectName = Path.Combine(rootFilePath + newAdditionalPath, newObjectName) + ".asset";
                    AssetDatabase.CreateAsset(newScriptable, scriptableObjectName);
                    characterData = newScriptable;
                }
            }
            GUI.enabled = isGUIEnabled;
        }
        
        EditorGUILayout.Space(20);
        
        EditorGUILayout.LabelField("Selected Character Data", titleStyle);
        EditorGUILayout.Space(5);
        characterData = EditorGUILayout.ObjectField(characterData, typeof(CharacterData), true) as CharacterData;
        
        EditorGUILayout.Space(20);
        
        if (characterData != null)
        {
            Editor.CreateCachedEditor(characterData, typeof(CharacterDataEditor), ref characterDataEditor);
            ((CharacterDataEditor)characterDataEditor).SetCharacterData(characterData);
            characterDataEditor.OnInspectorGUI();

            movementSelectedIndex = 0;
            movementData = null;
            dodgeData = characterData.DodgeData;
            hitstunData = characterData.HitstunData;
            physicsData = characterData.GroundedPhysicsData;

            if(characterData.StartingRightHandEquipment != null)
                equipmentData = characterData.StartingRightHandEquipment;
            else
                equipmentData = characterData.StartingLeftHandEquipment;
        }
    }
    
    private int attackSelectedIndex = 0;
    
    private void AttackPage()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("New Data Name: ", GUILayout.Width(100));
            newObjectName = EditorGUILayout.TextField(newObjectName);
            var isGUIEnabled = GUI.enabled;
            GUI.enabled = !string.IsNullOrEmpty(newObjectName);
            if (GUILayout.Button("Create New Attack Data",GUILayout.Width(150)))
            {
                var allAttackData = AssetDatabase.FindAssets("t: " + nameof(AttackData))
                    .Select(x => AssetDatabase.LoadAssetAtPath<AttackData>(AssetDatabase.GUIDToAssetPath(x)))
                    .ToList();

                bool objectExists = false;
                
                foreach (var item in allAttackData)
                {
                    if (item.name == newObjectName)
                    {
                        objectExists = true;
                    }
                }

                if (!objectExists)
                {
                    AttackData newScriptable = CreateInstance<AttackData>();
                    string newAdditionalPath = additionalFilePath + "/" + nameof(AttackData);
                    var folderPath = Path.Combine(Application.dataPath, newAdditionalPath);
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    var scriptableObjectName = Path.Combine(rootFilePath + newAdditionalPath, newObjectName) + ".asset";
                    AssetDatabase.CreateAsset(newScriptable, scriptableObjectName);
                    attackData = newScriptable;
                }
            }
            GUI.enabled = isGUIEnabled;
        }
        
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Selected Attack Data", titleStyle);
        EditorGUILayout.Space(5);
        attackData = EditorGUILayout.ObjectField(attackData, typeof(AttackData), true) as AttackData;
        
        EditorGUILayout.Space(20);

        if (comboData != null && comboData.AttacksList != null && comboData.AttacksList.Length > 0)
        {
            List<AttackData> attackDatasList = new List<AttackData>();

            foreach (var attack in comboData.AttacksList)
            {
                if(attack != null)
                    attackDatasList.Add(attack);
            }

            if (attackDatasList.Count > 0)
            {
                EditorGUILayout.LabelField("Current Attacks In Combo", titleStyle);
                EditorGUILayout.Space(5);

                var names = attackDatasList.Select(x => x.name).ToList();
                int previousIndex = attackSelectedIndex;
                attackSelectedIndex = EditorGUILayout.Popup(attackSelectedIndex, names.ToArray());
                if (previousIndex != attackSelectedIndex)
                {
                    attackData = null;
                    if (attackSelectedIndex >= 0)
                        attackData = attackDatasList[attackSelectedIndex];
                }
                EditorGUILayout.Space(20);
            }
        }
        
        if (attackData != null)
        {
            Editor.CreateCachedEditor(attackData, typeof(AttackDataEditor), ref attackDataEditor);
            ((AttackDataEditor)attackDataEditor).SetAttackData(attackData);
            attackDataEditor.OnInspectorGUI();
        }
    }
    
    private void BlockPage()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("New Data Name: ", GUILayout.Width(100));
            newObjectName = EditorGUILayout.TextField(newObjectName);
            var isGUIEnabled = GUI.enabled;
            GUI.enabled = !string.IsNullOrEmpty(newObjectName);
            if (GUILayout.Button("Create New Block Data",GUILayout.Width(150)))
            {
                var allCharacterData = AssetDatabase.FindAssets("t: " + nameof(BlockingData))
                    .Select(x => AssetDatabase.LoadAssetAtPath<BlockingData>(AssetDatabase.GUIDToAssetPath(x)))
                    .ToList();

                bool objectExists = false;
                
                foreach (var item in allCharacterData)
                {
                    if (item.name == newObjectName)
                    {
                        objectExists = true;
                    }
                }

                if (!objectExists)
                {
                    BlockingData newScriptable = CreateInstance<BlockingData>();
                    string newAdditionalPath = additionalFilePath + "/" + nameof(BlockingData);
                    var folderPath = Path.Combine(Application.dataPath, newAdditionalPath);
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    var scriptableObjectName = Path.Combine(rootFilePath + newAdditionalPath, newObjectName) + ".asset";
                    AssetDatabase.CreateAsset(newScriptable, scriptableObjectName);
                    blockingData = newScriptable;
                }
            }
            GUI.enabled = isGUIEnabled;
        }
        
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Selected Block Data", titleStyle);
        EditorGUILayout.Space(5);
        blockingData = EditorGUILayout.ObjectField(blockingData, typeof(BlockingData), true) as BlockingData;
        EditorGUILayout.Space(20);
        if (blockingData != null)
        {
            Editor.CreateCachedEditor(blockingData, typeof(BlockingDataEditor), ref blockingDataEditor);
            ((BlockingDataEditor)blockingDataEditor).SetBlockData(blockingData);
            blockingDataEditor.OnInspectorGUI();
        }
    }
    
    private int comboSelectedIndex = 0;
    
    private void ComboPage()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("New Data Name: ", GUILayout.Width(100));
            newObjectName = EditorGUILayout.TextField(newObjectName);
            var isGUIEnabled = GUI.enabled;
            GUI.enabled = !string.IsNullOrEmpty(newObjectName);
            if (GUILayout.Button("Create New Attack Combo Data",GUILayout.Width(200)))
            {
                var allCharacterData = AssetDatabase.FindAssets("t: " + nameof(AttackComboData))
                    .Select(x => AssetDatabase.LoadAssetAtPath<AttackComboData>(AssetDatabase.GUIDToAssetPath(x)))
                    .ToList();

                bool objectExists = false;
                
                foreach (var item in allCharacterData)
                {
                    if (item.name == newObjectName)
                    {
                        objectExists = true;
                    }
                }

                if (!objectExists)
                {
                    AttackComboData newScriptable = CreateInstance<AttackComboData>();
                    string newAdditionalPath = additionalFilePath + "/" + nameof(AttackComboData);
                    var folderPath = Path.Combine(Application.dataPath, newAdditionalPath);
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    var scriptableObjectName = Path.Combine(rootFilePath + newAdditionalPath, newObjectName) + ".asset";
                    AssetDatabase.CreateAsset(newScriptable, scriptableObjectName);
                    comboData = newScriptable;
                }
            }
            GUI.enabled = isGUIEnabled;
        }
        
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Selected Attack Combo Data", titleStyle);
        EditorGUILayout.Space(5);
        comboData = EditorGUILayout.ObjectField(comboData, typeof(AttackComboData), true) as AttackComboData;
        EditorGUILayout.Space(20);
        
        if (comboData != null && comboData.AttacksList != null && comboData.AttacksList.Length > 0)
        {
            EditorGUILayout.LabelField("Current Combos", titleStyle);
            EditorGUILayout.Space(5);

            List<AttackComboData> comboDatasList = new List<AttackComboData>();

            if(equipmentData.PrimaryAttackCombo != null)
                comboDatasList.Add(equipmentData.PrimaryAttackCombo);
            if(equipmentData.SecondaryAttackCombo != null)
                comboDatasList.Add(equipmentData.SecondaryAttackCombo);
            
            var names = comboDatasList.Select(x => x.name).ToList();
            int previousIndex = comboSelectedIndex;
            comboSelectedIndex = EditorGUILayout.Popup(comboSelectedIndex, names.ToArray());
            if (previousIndex != comboSelectedIndex)
            {
                comboData = null;
                if (comboSelectedIndex >= 0)
                    comboData = comboDatasList[comboSelectedIndex];
            }
            
            EditorGUILayout.Space(20);
        }
        
        if (comboData != null)
        {
            Editor.CreateCachedEditor(comboData, typeof(AttackComboDataEditor), ref comboDataEditor);
            ((AttackComboDataEditor)comboDataEditor).SetComboData(comboData);
            comboDataEditor.OnInspectorGUI();

            if (comboData.AttacksList != null && comboData.AttacksList.Length > 0)
            {
                attackSelectedIndex = 0;
                attackData = comboData.AttacksList[0];
            }
            else
                attackData = null;
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
            if (GUILayout.Button("Create New Dodge Data",GUILayout.Width(150)))
            {
                var allCharacterData = AssetDatabase.FindAssets("t: " + nameof(DodgeData))
                    .Select(x => AssetDatabase.LoadAssetAtPath<DodgeData>(AssetDatabase.GUIDToAssetPath(x)))
                    .ToList();

                bool objectExists = false;
                
                foreach (var item in allCharacterData)
                {
                    if (item.name == newObjectName)
                    {
                        objectExists = true;
                    }
                }

                if (!objectExists)
                {
                    DodgeData newScriptable = CreateInstance<DodgeData>();
                    string newAdditionalPath = additionalFilePath + "/" + nameof(DodgeData);
                    var folderPath = Path.Combine(Application.dataPath, newAdditionalPath);
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    var scriptableObjectName = Path.Combine(rootFilePath + newAdditionalPath, newObjectName) + ".asset";
                    AssetDatabase.CreateAsset(newScriptable, scriptableObjectName);
                    dodgeData = newScriptable;
                }
            }
            GUI.enabled = isGUIEnabled;
        }
        
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Selected Dodge Data", titleStyle);
        EditorGUILayout.Space(5);
        dodgeData = EditorGUILayout.ObjectField(dodgeData, typeof(DodgeData), true) as DodgeData;
        EditorGUILayout.Space(20);
        if (dodgeData != null)
        {
            Editor.CreateCachedEditor(dodgeData, typeof(DodgeDataEditor), ref dodgeDataEditor);
            ((DodgeDataEditor)dodgeDataEditor).SetDodgeData(dodgeData);
            dodgeDataEditor.OnInspectorGUI();

        }
    }
    
    private void EquipmentPage()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("New Data Name: ", GUILayout.Width(100));
            newObjectName = EditorGUILayout.TextField(newObjectName);
            var isGUIEnabled = GUI.enabled;
            GUI.enabled = !string.IsNullOrEmpty(newObjectName);
            if (GUILayout.Button("Create New Equipment Data",GUILayout.Width(200)))
            {
                var allEquipmentData = AssetDatabase.FindAssets("t: " + nameof(EquipmentData))
                    .Select(x => AssetDatabase.LoadAssetAtPath<EquipmentData>(AssetDatabase.GUIDToAssetPath(x)))
                    .ToList();

                bool objectExists = false;

                foreach (var item in allEquipmentData)
                {
                    if (item.name == newObjectName)
                    {
                        objectExists = true;
                    }
                }

                if (!objectExists)
                {
                    EquipmentData newScriptable = CreateInstance<EquipmentData>();
                    string newAdditionalPath = additionalFilePath + "/" + nameof(EquipmentData);
                    var folderPath = Path.Combine(Application.dataPath, newAdditionalPath);
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    var scriptableObjectName = Path.Combine(rootFilePath + newAdditionalPath, newObjectName) + ".asset";
                    AssetDatabase.CreateAsset(newScriptable, scriptableObjectName);
                    equipmentData = newScriptable;
                }
            }
            GUI.enabled = isGUIEnabled;
        }
        
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Selected Equipment Data", titleStyle);
        EditorGUILayout.Space(5);
        equipmentData = EditorGUILayout.ObjectField(equipmentData, typeof(EquipmentData), true) as EquipmentData;
        EditorGUILayout.Space(20);
        if (equipmentData != null)
        {
            Editor.CreateCachedEditor(equipmentData, typeof(EquipmentDataEditor), ref equipmentDataEditor);
            ((EquipmentDataEditor)equipmentDataEditor).SetEquipmentData(equipmentData);
            equipmentDataEditor.OnInspectorGUI();

            if (equipmentData.PrimaryAttackCombo != null)
            {
                comboSelectedIndex = 0;
                comboData = equipmentData.PrimaryAttackCombo;
                if (comboData.AttacksList.Length != 0 && comboData.AttacksList.Length > 0)
                {
                    attackSelectedIndex = 0;
                    attackData = comboData.AttacksList[0];
                }
            }
            else if (equipmentData.SecondaryAttackCombo != null)
            {
                comboSelectedIndex = 0;
                comboData = equipmentData.SecondaryAttackCombo;
                if (comboData.AttacksList.Length != 0 && comboData.AttacksList.Length > 0)
                {
                    attackSelectedIndex = 0;
                    attackData = comboData.AttacksList[0];
                }
            }
            else
            {
                comboData = null;
                attackData = null;
            }
            
            blockingData = equipmentData.BlockingData;
        }
    }
    
    private int movementSelectedIndex = 0;
    
    private void MovementPage()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("New Data Name: ", GUILayout.Width(100));
            newObjectName = EditorGUILayout.TextField(newObjectName);
            var isGUIEnabled = GUI.enabled;
            GUI.enabled = !string.IsNullOrEmpty(newObjectName);
            if (GUILayout.Button("Create New Movement Data",GUILayout.Width(200)))
            {
                var allCharacterData = AssetDatabase.FindAssets("t: " + nameof(MovementData))
                    .Select(x => AssetDatabase.LoadAssetAtPath<MovementData>(AssetDatabase.GUIDToAssetPath(x)))
                    .ToList();

                bool objectExists = false;
                
                foreach (var item in allCharacterData)
                {
                    if (item.name == newObjectName)
                    {
                        objectExists = true;
                    }
                }

                if (!objectExists)
                {
                    MovementData newScriptable = CreateInstance<MovementData>();
                    string newAdditionalPath = additionalFilePath + "/" + nameof(MovementData);
                    var folderPath = Path.Combine(Application.dataPath, newAdditionalPath);
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    var scriptableObjectName = Path.Combine(rootFilePath + newAdditionalPath, newObjectName) + ".asset";
                    AssetDatabase.CreateAsset(newScriptable, scriptableObjectName);
                    movementData = newScriptable;
                }
            }
            GUI.enabled = isGUIEnabled;
        }
        
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Selected Movement Data", titleStyle);
        EditorGUILayout.Space(5);
        movementData = EditorGUILayout.ObjectField(movementData, typeof(MovementData), true) as MovementData;

        EditorGUILayout.Space(20);
        
        if (characterData != null && (characterData.WalkingMovementData != null || characterData.SprintingMovementData != null || characterData.AirbornMovementData != null))
        {
            EditorGUILayout.LabelField("Character Current Movements", titleStyle);
            EditorGUILayout.Space(5);

            List<MovementData> movementDatasList = new List<MovementData>();

            if (characterData.WalkingMovementData != null)
                movementDatasList.Add(characterData.WalkingMovementData);
            if (characterData.SprintingMovementData != null)
                movementDatasList.Add(characterData.SprintingMovementData);
            if (characterData.AirbornMovementData != null)
                movementDatasList.Add(characterData.AirbornMovementData);

            if (movementDatasList.Count >= 1 && movementData == null)
            {
                movementData = movementDatasList[0];
            }
            
            var names = movementDatasList.Select(x => x.name).ToList();
            int previousIndex = movementSelectedIndex;
            movementSelectedIndex = EditorGUILayout.Popup(movementSelectedIndex, names.ToArray());
            if (previousIndex != movementSelectedIndex)
            {
                movementData = null;
                if (movementSelectedIndex >= 0)
                    movementData = movementDatasList[movementSelectedIndex];
            }

            EditorGUILayout.Space(20);
        }
        
        if (movementData != null)
        {
            Editor.CreateCachedEditor(movementData, typeof(MovementDataEditor), ref movementDataEditor);
            ((MovementDataEditor)movementDataEditor).SetMovementData(movementData);
            movementDataEditor.OnInspectorGUI();
        }
    }
    
    private void PhysicsPage()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("New Data Name: ", GUILayout.Width(100));
            newObjectName = EditorGUILayout.TextField(newObjectName);
            var isGUIEnabled = GUI.enabled;
            GUI.enabled = !string.IsNullOrEmpty(newObjectName);
            if (GUILayout.Button("Create New Physics Data",GUILayout.Width(200)))
            {
                var allCharacterData = AssetDatabase.FindAssets("t: " + nameof(PhysicsData))
                    .Select(x => AssetDatabase.LoadAssetAtPath<PhysicsData>(AssetDatabase.GUIDToAssetPath(x)))
                    .ToList();

                bool objectExists = false;
                
                foreach (var item in allCharacterData)
                {
                    if (item.name == newObjectName)
                    {
                        objectExists = true;
                    }
                }

                if (!objectExists)
                {
                    PhysicsData newScriptable = CreateInstance<PhysicsData>();
                    string newAdditionalPath = additionalFilePath + "/" + nameof(PhysicsData);
                    var folderPath = Path.Combine(Application.dataPath, newAdditionalPath);
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    var scriptableObjectName = Path.Combine(rootFilePath + newAdditionalPath, newObjectName) + ".asset";
                    AssetDatabase.CreateAsset(newScriptable, scriptableObjectName);
                    physicsData = newScriptable;
                }
            }
            GUI.enabled = isGUIEnabled;
        }
        
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Selected Physics Data", titleStyle);
        EditorGUILayout.Space(5);
        physicsData = EditorGUILayout.ObjectField(physicsData, typeof(PhysicsData), true) as PhysicsData;
        EditorGUILayout.Space(20);
        if (physicsData != null)
        {
            Editor.CreateCachedEditor(physicsData, typeof(PhysicsDataEditor), ref physicsDataEditor);
            ((PhysicsDataEditor)physicsDataEditor).SetPhysicsData(physicsData);
            physicsDataEditor.OnInspectorGUI();

        }
    }
    
    private void HitstunPage()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("New Data Name: ", GUILayout.Width(100));
            newObjectName = EditorGUILayout.TextField(newObjectName);
            var isGUIEnabled = GUI.enabled;
            GUI.enabled = !string.IsNullOrEmpty(newObjectName);
            if (GUILayout.Button("Create New Hitstun Data",GUILayout.Width(150)))
            {
                var allAttackData = AssetDatabase.FindAssets("t: " + nameof(HitstunData))
                    .Select(x => AssetDatabase.LoadAssetAtPath<HitstunData>(AssetDatabase.GUIDToAssetPath(x)))
                    .ToList();

                bool objectExists = false;
                
                foreach (var item in allAttackData)
                {
                    if (item.name == newObjectName)
                    {
                        objectExists = true;
                    }
                }

                if (!objectExists)
                {
                    HitstunData newScriptable = CreateInstance<HitstunData>();
                    string newAdditionalPath = additionalFilePath + "/" + nameof(HitstunData);
                    var folderPath = Path.Combine(Application.dataPath, newAdditionalPath);
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    var scriptableObjectName = Path.Combine(rootFilePath + newAdditionalPath, newObjectName) + ".asset";
                    AssetDatabase.CreateAsset(newScriptable, scriptableObjectName);
                    hitstunData = newScriptable;
                }
            }
            GUI.enabled = isGUIEnabled;
        }
        
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Selected Hitstun Data", titleStyle);
        EditorGUILayout.Space(5);
        hitstunData = EditorGUILayout.ObjectField(hitstunData, typeof(HitstunData), true) as HitstunData;
        EditorGUILayout.Space(20);
        if (hitstunData != null)
        {
            Editor.CreateCachedEditor(hitstunData, typeof(HitstunDataEditor), ref hitstunDataEditor);
            ((HitstunDataEditor)hitstunDataEditor).SetHitstunData(hitstunData);
            hitstunDataEditor.OnInspectorGUI();
        }
    }
}