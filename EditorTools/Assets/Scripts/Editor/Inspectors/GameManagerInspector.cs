using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class GameManagerInspector : Editor
{
    private static readonly string[] EXCLUDED_PROPERTIES = new string[]
    {
        "m_Script",
        "gameData"
    };

    private void OnEnable()
    {
        SerializedProperty gameDataProperty = serializedObject.FindProperty("gameData");
        if (null == gameDataProperty.objectReferenceValue) {
            serializedObject.Update();
            gameDataProperty.objectReferenceValue = _FindGameDataInProject();
            serializedObject.ApplyModifiedProperties();
        }
    }

    private GameData _FindGameDataInProject()
    {
        string[] fileGuidsArr = AssetDatabase.FindAssets("t:" + typeof(GameData));
        if (fileGuidsArr.Length > 0) {
            string assetPath = AssetDatabase.GUIDToAssetPath(fileGuidsArr[0]);
            return AssetDatabase.LoadAssetAtPath<GameData>(assetPath);
        } else {
            GameData gameData = ScriptableObject.CreateInstance<GameData>();
            AssetDatabase.CreateAsset(gameData, "Assets/GameData.asset");
            AssetDatabase.SaveAssets();
            return gameData;
        }
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("gameData"));
        EditorGUI.EndDisabledGroup();

        //Standard Editor Space
        EditorGUILayout.Space();

        if (GUILayout.Button("Edit GameData")) {
            Selection.activeObject = serializedObject.FindProperty("gameData").objectReferenceValue;
        }

        //Custom Space
        GUILayout.Space(20);

        DrawPropertiesExcluding(serializedObject, EXCLUDED_PROPERTIES);
    }
}
