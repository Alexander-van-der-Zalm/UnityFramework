using UnityEngine;
using UnityEditor;
using System;

namespace Framework.Input
{
    #region Interfaces & Classes

    [System.Serializable]
    public class KeyCodeEditorGUI
    {
        [SerializeField]
        private KeyCode m_KeyCode;
        [SerializeField]
        private string m_IncompleteKeyCode;

        public void OnGUI(Rect pos, SerializedProperty keyString, string label, bool IgnoreIndent = true)
        {
            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            if (IgnoreIndent)
                EditorGUI.indentLevel = 0;

            float splitWidth = (pos.width - 3) / 3;

            // Make Rects
            Rect valueRect = new Rect(pos.x, pos.y, splitWidth, EditorGUIUtility.singleLineHeight);
            Rect textRect = new Rect(pos.x + 1 + splitWidth, pos.y, splitWidth, EditorGUIUtility.singleLineHeight);
            Rect enumRect = new Rect(pos.x + 2 + 2 * splitWidth, pos.y, splitWidth, EditorGUIUtility.singleLineHeight);

            // If just switched type or new
            if (keyString.stringValue == "" || !Enum.IsDefined(typeof(KeyCode), keyString.stringValue))
            {
                keyString.stringValue = ((KeyCode)0).ToString();
            }
            // If just (re)loaded
            if (m_KeyCode.ToString() != keyString.stringValue)
            {
                m_KeyCode = InputHelper.ReturnKeyCode(keyString.stringValue);
                m_IncompleteKeyCode = keyString.stringValue;
            }

            // Enum change check & resolve
            EditorGUI.BeginChangeCheck();
            m_KeyCode = (KeyCode)EditorGUI.EnumPopup(enumRect, m_KeyCode);
            if (EditorGUI.EndChangeCheck())
            {
                keyString.stringValue = m_KeyCode.ToString();
                m_IncompleteKeyCode = keyString.stringValue;
            }

            // Text field change check & resolve
            EditorGUI.BeginChangeCheck();
            m_IncompleteKeyCode = EditorGUI.TextField(textRect, m_IncompleteKeyCode);
            // Check if it is a valid input
            if (EditorGUI.EndChangeCheck())
            {
                // Check if to Upper works
                if (Enum.IsDefined(typeof(KeyCode), m_IncompleteKeyCode.ToUpper()))
                {
                    m_IncompleteKeyCode = m_IncompleteKeyCode.ToUpper();
                }

                // Check if it is correct
                if (Enum.IsDefined(typeof(KeyCode), m_IncompleteKeyCode))
                {
                    keyString.stringValue = m_IncompleteKeyCode;
                    m_KeyCode = InputHelper.ReturnKeyCode(m_IncompleteKeyCode);
                }
            }

            EditorGUI.PropertyField(valueRect, keyString, GUIContent.none);

            // Set indent back to what it was
            if (IgnoreIndent)
                EditorGUI.indentLevel = indent;
        }
    }

    [System.Serializable]
    public class EnumEditorGUI<T> where T : struct, IConvertible, IComparable, IFormattable
    {
        [SerializeField]
        private T m_Enum;

        public void OnGUI(Rect pos, SerializedProperty kp, string label, bool IgnoreIndent = true)
        {
            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            if (IgnoreIndent)
                EditorGUI.indentLevel = 0;

            float splitWidth = (pos.width - 1) / 3;

            Rect valueRect = new Rect(pos.x, pos.y, splitWidth, EditorGUIUtility.singleLineHeight);
            Rect xboxEnumRext = new Rect(pos.x + 1 + splitWidth, pos.y, 2 * splitWidth, EditorGUIUtility.singleLineHeight);

            // When just switched from type, new or input is invalid
            if (kp.stringValue == "" || !Enum.IsDefined(typeof(T), kp.stringValue))//Enum.Parse(typeof(XboxAxis),kp.stringValue)))
            {
                kp.stringValue = Enum.GetNames(typeof(T))[0];
            }

            // If just loaded
            if (m_Enum.ToString() != kp.stringValue)
            {
                m_Enum = InputHelper.ParseEnum<T>(kp.stringValue);
            }

            // Xbox enum
            EditorGUI.BeginChangeCheck();
            var res = EditorGUI.EnumPopup(xboxEnumRext, m_Enum as Enum);

            if (EditorGUI.EndChangeCheck())
            {
                m_Enum = InputHelper.ParseEnum<T>(res.ToString());
                kp.stringValue = m_Enum.ToString();
            }



            EditorGUI.PropertyField(valueRect, kp, GUIContent.none);

            // Set indent back to what it was
            if (IgnoreIndent)
                EditorGUI.indentLevel = indent;
        }
    }

    #endregion

    [CustomPropertyDrawer(typeof(InputActionKey))]
    public class InputActionKeyPD : PropertyDrawer
    {
        [SerializeField]
        private XboxButton m_XboxEnum;
        [SerializeField]
        private KeyCode m_KeyCode;
        [SerializeField]
        private string m_IncompleteKeyCode;

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            SerializedProperty tp = prop.FindPropertyRelative("Type");
            SerializedProperty kp = prop.FindPropertyRelative("KeyValue");

            // Prefix Label
            if (!label.text.Contains("Element"))
                pos = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float splitWidth = (pos.width - 45 - 3) / 3;

            // Rects
            Rect typeRect = new Rect(pos.x, pos.y, 45, EditorGUIUtility.singleLineHeight);
            Rect valueRect = new Rect(pos.x + 45 + 2, pos.y, splitWidth, EditorGUIUtility.singleLineHeight);
            Rect textRect = new Rect(pos.x + 45 + 3 + splitWidth, pos.y, splitWidth, EditorGUIUtility.singleLineHeight);
            Rect enumRect = new Rect(pos.x + 45 + 4 + 2 * splitWidth, pos.y, splitWidth, EditorGUIUtility.singleLineHeight);
            Rect xboxEnumRext = new Rect(pos.x + 45 + 3 + splitWidth, pos.y, 2 * splitWidth, EditorGUIUtility.singleLineHeight);

            // Draw
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(typeRect, tp, GUIContent.none);
            if (EditorGUI.EndChangeCheck())
            {
                kp.stringValue = "";// Switched type
            }
            ControlType type = (ControlType)Enum.Parse(typeof(ControlType), tp.enumNames[tp.enumValueIndex], true);

            if (type == ControlType.PC)
            {
                // If just switched type
                if (kp.stringValue == "")
                {
                    kp.stringValue = ((KeyCode)0).ToString();
                }
                // If just loaded
                if (m_KeyCode.ToString() != kp.stringValue)
                {
                    m_KeyCode = InputHelper.ReturnKeyCode(kp.stringValue);
                    m_IncompleteKeyCode = kp.stringValue;
                }

                // Enum change check & resolve
                EditorGUI.BeginChangeCheck();
                m_KeyCode = (KeyCode)EditorGUI.EnumPopup(enumRect, m_KeyCode);
                if (EditorGUI.EndChangeCheck())
                {
                    kp.stringValue = m_KeyCode.ToString();
                    m_IncompleteKeyCode = kp.stringValue;
                }

                // Text field change check & resolve
                EditorGUI.BeginChangeCheck();
                m_IncompleteKeyCode = EditorGUI.TextField(textRect, m_IncompleteKeyCode);
                // Check if it is a valid input
                if (EditorGUI.EndChangeCheck())
                {
                    // Check if to Upper works
                    if (Enum.IsDefined(typeof(KeyCode), m_IncompleteKeyCode.ToUpper()))
                    {
                        m_IncompleteKeyCode = m_IncompleteKeyCode.ToUpper();
                    }

                    // Check if it is correct
                    if (Enum.IsDefined(typeof(KeyCode), m_IncompleteKeyCode))
                    {
                        kp.stringValue = m_IncompleteKeyCode;
                        m_KeyCode = InputHelper.ReturnKeyCode(m_IncompleteKeyCode);
                    }
                }
            }
            else // XBOX
            {
                // When just switched from type or new
                if (kp.stringValue == "")
                {
                    kp.stringValue = ((XboxButton)0).ToString();
                }
                // If just loaded
                if (m_XboxEnum.ToString() != kp.stringValue)
                {
                    m_XboxEnum = InputHelper.ReturnXboxButton(kp.stringValue);
                }

                // Xbox enum
                EditorGUI.BeginChangeCheck();
                m_XboxEnum = (XboxButton)EditorGUI.EnumPopup(xboxEnumRext, m_XboxEnum);
                if (EditorGUI.EndChangeCheck())
                    kp.stringValue = m_XboxEnum.ToString();
            }
            EditorGUI.PropertyField(valueRect, kp, GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;
        }
    }
}
