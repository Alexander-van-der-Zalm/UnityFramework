using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OneLineFieldsAttribute : PropertyAttribute
{
    public float[] memberSizeWeights;

    public OneLineFieldsAttribute()
    {
        memberSizeWeights = new float[0];
    }

    public OneLineFieldsAttribute(float[] memberSizeWeights)
    {
        this.memberSizeWeights = memberSizeWeights;
    }
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(OneLineFieldsAttribute))]
public class OneLineFieldAttributePropertyDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        OneLineFieldsAttribute oneline = attribute as OneLineFieldsAttribute;

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;                             // Start fields prep
        EditorGUI.indentLevel = 0;
       
        int memberAmount = Mathf.Max(1, property.GetMemberCount());

        float totalWidthWeights = 0;
        for (int i = 0; i < memberAmount; i++)                          // Sum up all the weights (undifined = 1)
        {
            if(i < oneline.memberSizeWeights.Length)
                totalWidthWeights += Mathf.Max(0,Mathf.Abs(oneline.memberSizeWeights[i]));
            else
                totalWidthWeights += 1;
        }
        totalWidthWeights = Mathf.Max(totalWidthWeights, 0.00000001f);  // prevent division by 0
        float widthPart = position.width / totalWidthWeights;           // total width = totalweights * widthPart

        int j = 0;
        property.IterateFieldsSmart(x =>
        {
            // Find current width
            float currentWidthWeight = j < oneline.memberSizeWeights.Length ? oneline.memberSizeWeights[j] : 1;

            // Negative numbers make it readonly
            bool readOnly = currentWidthWeight < 0;
            currentWidthWeight = Mathf.Abs(currentWidthWeight);

            if (currentWidthWeight > 0)
            {
                position.width = widthPart * currentWidthWeight;

                if(readOnly) GUI.enabled = false;
                EditorGUI.PropertyField(position, x, GUIContent.none, false);
                GUI.enabled = true;

                position.x += position.width;
            }

            j++;
        });

        EditorGUI.indentLevel = indent;
    }
}
#endif