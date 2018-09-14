using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MinMaxRange))]
public class MinMaxRangePropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Setup vars
        float textFieldWidth = 30;

        SerializedProperty minLimitSP   = property.FindPropertyRelative("minLimit");
        SerializedProperty maxLimitSP   = property.FindPropertyRelative("maxLimit");
        SerializedProperty minSP        = property.FindPropertyRelative("Min");
        SerializedProperty maxSP        = property.FindPropertyRelative("Max");

        float minLimit  = minLimitSP.floatValue;
        float maxLimit  = maxLimitSP.floatValue;
        float min       = minSP.floatValue;
        float max       = maxSP.floatValue;

        //Debug.Log(minLimit + " " +  maxLimit + " " + min + " " + max);

        // Calculate rects
        Rect sliderPos = position;
        sliderPos.x += EditorGUIUtility.labelWidth + textFieldWidth; //EditorGUIUtility.labelWidth + textFieldWidth * 2;
        sliderPos.width -= EditorGUIUtility.labelWidth + textFieldWidth * 2;

        Rect minPos = position;
        minPos.x += EditorGUIUtility.labelWidth;
        minPos.width = textFieldWidth;

        Rect maxPos = position;
        maxPos.x += maxPos.width - textFieldWidth;
        maxPos.width = textFieldWidth;

        // Draw Elements
        EditorGUI.LabelField(position, label);

        EditorGUI.BeginChangeCheck();
        {
            EditorGUI.MinMaxSlider(sliderPos, ref min, ref max, minLimit, maxLimit);
        }
        if (EditorGUI.EndChangeCheck())
        {
            if (maxLimit - minLimit > 25.0f)
            {
                min = Mathf.Round(min);
                max = Mathf.Round(max);
            }
            minSP.floatValue = min;
            maxSP.floatValue = max;
        }

        EditorGUI.BeginChangeCheck();
        {
            min = EditorGUI.FloatField(minPos, min);
            max = EditorGUI.FloatField(maxPos, max);
        }
        if (EditorGUI.EndChangeCheck())
        {
            minSP.floatValue = min;
            maxSP.floatValue = max;
            Debug.Log(minLimit + " " + maxLimit + " " + min + " " + max);
        }
    }
}