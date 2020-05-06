using UnityEngine;
using UnityEditor;

public static class ToolBoxEditorGUILayout
{
    public static void BeginBox(string title)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
        EditorGUILayout.Space();
    }

    public static void EndBox()
    {
        EditorGUILayout.EndVertical();
    }
}
