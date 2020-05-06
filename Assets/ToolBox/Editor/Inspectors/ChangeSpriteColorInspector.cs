using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChangeSpriteColor))]
public class ChangeSpriteColorInspector : Editor
{

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("Change Color Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        SerializedProperty changeModeProperty = serializedObject.FindProperty("changeMode");
        EditorGUILayout.PropertyField(changeModeProperty);

        ChangeSpriteColor.CHANGE_MODE changeMode = (ChangeSpriteColor.CHANGE_MODE)changeModeProperty.intValue;
        if (changeMode == ChangeSpriteColor.CHANGE_MODE.CUSTOM)
        {
            SerializedProperty customProperty = serializedObject.FindProperty("customColor");
            EditorGUILayout.PropertyField(customProperty);
        }

        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();

        if(changeMode== ChangeSpriteColor.CHANGE_MODE.RANDOM)
        {
            EditorGUILayout.HelpBox("Color Will Be Randomize. Be Carefull.", MessageType.Info);
        }

        if (GUILayout.Button("Change Color"))
        {
            ChangeSpriteColor changeColorScript = target as ChangeSpriteColor;

            switch (changeMode)
            {
                case ChangeSpriteColor.CHANGE_MODE.RANDOM:
                    Color randoColor = new Color();
                    randoColor.r = Random.Range(0f, 1f);
                    randoColor.g = Random.Range(0f, 1f);
                    randoColor.b = Random.Range(0f, 1f);
                    randoColor.a = 1f;
                    changeColorScript.GetComponent<SpriteRenderer>().color = randoColor;
                    break;

                case ChangeSpriteColor.CHANGE_MODE.CUSTOM:
                    changeColorScript.GetComponent<SpriteRenderer>().color = changeColorScript.customColor;
                    break;

            }
        }
    }

}
