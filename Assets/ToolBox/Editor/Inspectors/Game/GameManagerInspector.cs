using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]

public class GameManagerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SerializedProperty gameData = serializedObject.FindProperty("gameData");
        if(null == gameData.objectReferenceValue)
        {
            serializedObject.Update();
            gameData.objectReferenceValue = _FindGameDataInProject();
            serializedObject.ApplyModifiedProperties();
        }
        
    }

    private GameData _FindGameDataInProject()
    {
        string[] fileGuId = AssetDatabase.FindAssets("t:" + typeof(GameData));
        if (fileGuId.Length > 0)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(fileGuId[0]);
            return AssetDatabase.LoadAssetAtPath<GameData>(assetPath);
        }
        else
        {
            GameData gameDataA = CreateInstance<GameData>();
            AssetDatabase.CreateAsset(gameDataA, "Assets/GameData.asset");
            AssetDatabase.SaveAssets();
            return gameDataA;
        }
    }
}
