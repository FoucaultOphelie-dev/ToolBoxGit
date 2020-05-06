using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ToolBoxEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(MoveArround2D))]
public class MoveArround2DInspector : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        serializedObject.Update();

        ToolBoxEditorGUILayout.BeginBox("Change Basic Settings");

        SerializedProperty speed = serializedObject.FindProperty("speed");
        EditorGUILayout.PropertyField(speed);

        SerializedProperty radius = serializedObject.FindProperty("_radius");
        EditorGUILayout.PropertyField(radius);

        ToolBoxEditorGUILayout.EndBox();

        ToolBoxEditorGUILayout.BeginBox("Debug GUI");

        SerializedProperty guiDebug = serializedObject.FindProperty("guiDebug");
        EditorGUILayout.PropertyField(guiDebug);

        if (guiDebug.boolValue)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("guiDebugTextColor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("guiDebugTextSize"));
        }

        ToolBoxEditorGUILayout.EndBox();

        ToolBoxEditorGUILayout.BeginBox("Debug Gizmos");

        SerializedProperty gizmoDebug = serializedObject.FindProperty("gizmoDebug");
        EditorGUILayout.PropertyField(gizmoDebug);

        if (gizmoDebug.boolValue)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmosCenterColor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmosPositionColor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmosLineColor"));
        }
        ToolBoxEditorGUILayout.EndBox();


        serializedObject.ApplyModifiedProperties();

    }

    [DrawGizmo(GizmoType.InSelectionHierarchy|GizmoType.NotInSelectionHierarchy|GizmoType.Pickable)]
    private static void DrawGizmos(MoveArround2D moveScript, GizmoType gizmoType)
    {
        if (!moveScript.gizmoDebug) return;

        //Draw the position of the center
        Gizmos.color = moveScript.gizmosCenterColor;
        Gizmos.DrawWireSphere(moveScript.GetCenter(), 0.1f);

        //Draw the position of the object
        Gizmos.color = moveScript.gizmosPositionColor;
        Gizmos.DrawWireSphere(moveScript.transform.position, 0.1f);

        //Draw line bestween our two points
        Gizmos.color = moveScript.gizmosLineColor;
        Gizmos.DrawLine(moveScript.GetCenter(), moveScript.transform.position);
    }
}
