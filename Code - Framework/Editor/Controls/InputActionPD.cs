using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;

namespace Framework.Input
{
    [CustomPropertyDrawer(typeof(InputAction))]
    public class InputActionPD : PropertyDrawer
    {
        private ReorderableList list;
        private string ActionName;

        private void Init(SerializedProperty prop, GUIContent label)
        {
            if (list == null)
            {
                //Debug.Log(label.text);
                list = new ReorderableList(prop.serializedObject, prop.FindPropertyRelative("Keys"), true, true, true, true);
                list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = list.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(rect, element);
                //Debug.Log(element.propertyPath);
            };
                list.drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, "Action - " + label.text);
                };
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            Init(prop, label);

            return EditorGUIUtility.singleLineHeight + list.GetHeight();
        }

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Init(prop, label);
            list.DoList(pos);
        }
    }
}
