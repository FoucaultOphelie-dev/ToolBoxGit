using System;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(Path2D))]
public class Path2DInpector : Editor
{
    private GUIStyle _pointLabelStyle = null;
    private ReorderableList _pointsReorderableList = null;

    private Tool _lastUsedTool = Tool.None;

    private void OnEnable()
    {
        _pointsReorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("points"));
        _pointsReorderableList.drawElementCallback = _OnDrawPointsListElement;

        _pointLabelStyle = new GUIStyle();
        _pointLabelStyle.normal.textColor = Color.yellow;
        _pointLabelStyle.fontSize = 16;

        _lastUsedTool = Tools.current;
        Tools.current = Tool.None;
    }

    private void _OnDrawPointsListElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        EditorGUI.PropertyField(rect, _pointsReorderableList.serializedProperty.GetArrayElementAtIndex(index),GUIContent.none);
    }
    private void OnDisable()
    {
        _pointsReorderableList = null;

        if(Tools.current == Tool.None)
        {
            Tools.current = _lastUsedTool;
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, "points");

        _pointsReorderableList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        Path2D path2D = target as Path2D;
        if (null == path2D) return; //arrive tres souvent

        //Edit points
        for (int i = 0; i < path2D.points.Length; i++)
        {
            Vector2 point = path2D.points[i];
            EditorGUI.BeginChangeCheck();
            //point = Handles.PositionHandle(point, Quaternion.identity);
            point = Handles.FreeMoveHandle(point, Quaternion.identity,0.5f, Vector3.one, Handles.SphereHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(path2D, "Edit Path Point" + (i + 1));
                path2D.points[i] = point;
            }
        }

        //Draw points
        Handles.color = Color.yellow;
        for (int i = 0; i < path2D.points.Length; i++)
        {
            Vector2 point = path2D.points[i];
            Handles.DrawSolidDisc(point, Vector3.forward, 0.1f);

            Vector2 LabelPos = point;
            LabelPos.y -= 0.2f;
            Handles.Label(LabelPos, (i + 1).ToString(), _pointLabelStyle);

            if (i > 0)
            {
                Vector2 previewPoint = path2D.points[i - 1];
                Handles.DrawLine(previewPoint, point);
            }
        }

        if (path2D.isLooping && path2D.points.Length > 1)
        {
            Handles.DrawLine(path2D.points[path2D.points.Length - 1], path2D.points[0]);
        }

        if(Event.current.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(0);
        }
    }
}
