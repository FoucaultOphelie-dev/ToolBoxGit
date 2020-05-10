using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class HierarchyHighlighterItemGUI
{
    static HierarchyHighlighterItemGUI()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItem_CB;
    }

    private static void HierarchyWindowItem_CB(int selectionID, Rect selectionRect)
    {
        Object o = EditorUtility.InstanceIDToObject(selectionID);
        GameObject go = o as GameObject;
        if( null !=go && go.activeSelf)
        {
            HierarchyHighlighter h = go.GetComponent<HierarchyHighlighter>();
            if( null != h && h.highlight)
            {
                if (Event.current.type == EventType.Repaint)
                {
                    GUI.backgroundColor = h.backgroundColor;
                    GUI.Box(selectionRect, "");
                    GUIStyle textStyle = new GUIStyle();
                    textStyle.normal.textColor = h.textColor;
                    textStyle.alignment = h.testAlignement;
                    textStyle.padding.left = h.paddingLeft;
                    textStyle.padding.right = h.paddingRight;
                    textStyle.padding.top = h.paddingTop;
                    textStyle.padding.bottom = h.paddingBottom;
                    textStyle.fontStyle = FontStyle.Bold;
                    GUI.Label(selectionRect, go.name, textStyle);
                }
            }
        }
    }
}
