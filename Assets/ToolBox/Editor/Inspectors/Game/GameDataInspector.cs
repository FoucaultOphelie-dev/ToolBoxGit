using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameData))]

public class GameDataInspector : Editor
{
    private SerializedProperty _nbPlayer;
    private SerializedProperty _player1Speed;
    private SerializedProperty _player2Speed;
    private SerializedProperty _player3Speed;
    private SerializedProperty _player4Speed;

    private void OnEnable()
    {
        _nbPlayer = serializedObject.FindProperty("nbPlayer");
        _player1Speed = serializedObject.FindProperty("player1Speed");
        _player2Speed = serializedObject.FindProperty("player2Speed");
        _player3Speed = serializedObject.FindProperty("player3Speed");
        _player4Speed = serializedObject.FindProperty("player4Speed");
    }

    private void OnDisable()
    {
        _nbPlayer = null;
        _player1Speed = null;
        _player2Speed = null;
        _player3Speed = null;
        _player4Speed = null;
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //EditorGUILayout.IntSlider(_nbPlayer, 0, 4);
        EditorGUILayout.PropertyField(_nbPlayer);

        int nbPlayers = _nbPlayer.intValue;
        if (nbPlayers >= 1)
        {
            EditorGUILayout.PropertyField(_player1Speed);
        }

        if (nbPlayers >= 2)
        {
            EditorGUILayout.PropertyField(_player2Speed);
        }

        if (nbPlayers >= 3)
        {
            EditorGUILayout.PropertyField(_player3Speed);
        }

        if (nbPlayers >= 4)
        {
            EditorGUILayout.PropertyField(_player4Speed);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
