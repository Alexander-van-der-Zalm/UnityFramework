using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;

namespace Framework.Input
{
    [CustomPropertyDrawer(typeof(InputAxis))]
    public class InputAxisPD : PropertyDrawer
    {
        [SerializeField]
        private ReorderableList list;
        [SerializeField]
        private string headerLabel;

        private void Init(SerializedProperty prop, GUIContent label)
        {
            if (list == null)
            {
                headerLabel = label.text; // Save right label

                list = new ReorderableList(prop.serializedObject, prop.FindPropertyRelative("AxisKeys"), true, true, true, true);
                list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = list.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(rect, element);
                };
                list.drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, "Axis - " + headerLabel);
                };
                list.elementHeight = EditorGUIUtility.singleLineHeight + 2;
                list.elementHeightCallback = (index) =>
                 {
                     var element = list.serializedProperty.GetArrayElementAtIndex(index);
                     return EditorGUI.GetPropertyHeight(element) + 2;
                 };
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            Init(prop, label);

            return EditorGUIUtility.singleLineHeight * 2 + list.GetHeight();
        }

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Init(prop, label);
            list.DoList(pos);
            pos.y += list.GetHeight();

            //SerializedObject so = prop.
            InputAxis ax = fieldInfo.GetValue(prop.serializedObject.targetObject) as InputAxis;// prop as System.Object as Axis;
                                                                                               //prop.serializedObject.F
            if (GUI.Button(new Rect(pos.x, pos.y, pos.width / 2, EditorGUIUtility.singleLineHeight), "Default Horizontal"))
            {
                ax.AxisKeys = new System.Collections.Generic.List<InputAxisKey>();
                ax.DefaultInput(DirectionInput.Horizontal);
            }
            pos.x += pos.width / 2;
            if (GUI.Button(new Rect(pos.x, pos.y, pos.width / 2, EditorGUIUtility.singleLineHeight), "Default Vertical"))
            {
                ax.AxisKeys = new System.Collections.Generic.List<InputAxisKey>();
                ax.DefaultInput(DirectionInput.Vertical);
            }
        }
    }
}
