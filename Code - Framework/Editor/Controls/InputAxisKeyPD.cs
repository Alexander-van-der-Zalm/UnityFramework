using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

namespace Framework.Input
{
    [CustomPropertyDrawer(typeof(InputAxisKey))]
    public class InputAxisKeyPD : PropertyDrawer
    {
        KeyCodeEditorGUI m_KCNeg, m_KCPos;
        EnumEditorGUI<XboxAxis> m_XboxAxis;
        EnumEditorGUI<XboxDPad> m_XDpadNeg, m_XDpadPos;

        private void Init(SerializedProperty keys)
        {
            if (keys.arraySize == 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    keys.arraySize++;
                    keys.GetArrayElementAtIndex(i).stringValue = "";
                }
            }

            if (m_KCNeg == null)
                m_KCNeg = new KeyCodeEditorGUI();
            if (m_KCPos == null)
                m_KCPos = new KeyCodeEditorGUI();

            if (m_XboxAxis == null)
                m_XboxAxis = new EnumEditorGUI<XboxAxis>();

            if (m_XDpadNeg == null)
                m_XDpadNeg = new EnumEditorGUI<XboxDPad>();
            if (m_XDpadPos == null)
                m_XDpadPos = new EnumEditorGUI<XboxDPad>();
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            SerializedProperty tp = prop.FindPropertyRelative("Type");
            InputAxisKey.AxisKeyType type = (InputAxisKey.AxisKeyType)Enum.Parse(typeof(InputAxisKey.AxisKeyType), tp.enumNames[tp.enumValueIndex], true);
            if (type == InputAxisKey.AxisKeyType.Axis)
                return EditorGUIUtility.singleLineHeight * 1 + 2;
            return EditorGUIUtility.singleLineHeight * 2 + 2;
        }

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            // Draw prefix label - if not a list item
            bool listItem = label.text.Contains("Element");
            if (!listItem)
                pos = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);

            // Get relevant properties
            SerializedProperty keys = prop.FindPropertyRelative("keys");
            SerializedProperty tp = prop.FindPropertyRelative("Type");
            SerializedProperty k0 = keys.GetArrayElementAtIndex(0);
            SerializedProperty k1 = keys.GetArrayElementAtIndex(1);

            Init(keys);

            var indent = EditorGUI.indentLevel;
            if (!listItem)
                EditorGUI.indentLevel = 0;
            EditorGUI.PropertyField(new Rect(pos.x, pos.y, 45, EditorGUIUtility.singleLineHeight), tp, GUIContent.none);
            if (!listItem)
                EditorGUI.indentLevel = indent;

            InputAxisKey.AxisKeyType type = (InputAxisKey.AxisKeyType)Enum.Parse(typeof(InputAxisKey.AxisKeyType), tp.enumNames[tp.enumValueIndex], true);


            pos.x += 45;
            pos.width -= 45;
            if (k0 != null)
            {
                if (type == InputAxisKey.AxisKeyType.Axis)
                    m_XboxAxis.OnGUI(pos, k0, "", !listItem);
                else if (type == InputAxisKey.AxisKeyType.PC)
                    m_KCPos.OnGUI(pos, k0, "+", !listItem);
                else
                    m_XDpadPos.OnGUI(pos, k0, "+", !listItem);
            }
            pos.y += EditorGUIUtility.singleLineHeight;
            if (k1 != null)
            {
                if (type == InputAxisKey.AxisKeyType.PC)
                    m_KCNeg.OnGUI(pos, k1, "-", !listItem);
                else if (type == InputAxisKey.AxisKeyType.Dpad)
                    m_XDpadNeg.OnGUI(pos, k1, "-", !listItem);
            }
        }
    }
}