using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(CustomSliderPropertyAttribute))]
public class CustomDliderPropertyDrawer : PropertyDrawer
{
    const float WARNING_HEIGHT = 30f;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        CustomSliderPropertyAttribute sliderAttribute = attribute as CustomSliderPropertyAttribute;

        Rect sliderPosition = position;
        sliderPosition.height = EditorGUIUtility.singleLineHeight;
        EditorGUI.IntSlider(sliderPosition, property, sliderAttribute.GetMinValue(), sliderAttribute.GetMaxValue());

        if(property.intValue > 20)
        {
            Rect helpBoxPosition = position;
            helpBoxPosition.y += EditorGUIUtility.singleLineHeight;
            helpBoxPosition.height = WARNING_HEIGHT;
            EditorGUI.HelpBox(helpBoxPosition, "Value is big", MessageType.Warning);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.intValue > 20)
        {
            return EditorGUIUtility.singleLineHeight + WARNING_HEIGHT;
        }
        return base.GetPropertyHeight(property, label);
    }
}
