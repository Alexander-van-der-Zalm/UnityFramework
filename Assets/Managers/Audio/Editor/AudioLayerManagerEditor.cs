using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(AudioLayerManager))]
public class AudioLayerManagerEditor : EditorPlus
{
    //private GUIContent label = new GUIContent("AudioLayersEnum", "The layer names are hardcoded in the AudioLayerManager");
    private string settingsNameTooltip = "The layer names are hardcoded in the AudioLayerManager";
    //private static List<bool> folds = new List<bool>(0);

    public override void OnInspectorGUI()
    {
        //EditorGUI.up
        
        AudioLayerManager manager = target as AudioLayerManager;
        SerializedObject soManager = new SerializedObject(manager);
       // SerializedProperty spSettings = soManager.FindProperty("AudioLayerSettings");
        
        int count = manager.AudioLayerSettings.Count;

        //while (folds.Count < count)
        //    folds.Add(false);

        //EditorGUILayout.LabelField(label);
        for (int i = 0; i < count; i++)
        {
            AudioLayerSettings settings = manager.AudioLayerSettings[i]; 
            GUIContent name = new GUIContent(settings.Layer.ToString(), settingsNameTooltip);

            bool cont = true;
            SerializedProperty sp = soManager.GetIterator();
            while (cont)
            {
                //Debug.Log(i +  " + " + sp.name + " - " + sp.type + " - depth: " + sp.depth + " - " + sp.hasChildren);// +sp.arraySize + " - " + sp.
                //SerializedProperty sp2 = sp.GetArrayElementAtIndex(i);
                if (!sp.name.StartsWith("m_"))
                    Debug.Log(i + " + " + sp.name + " - " + sp.type + " - depth: " + sp.depth + " - " + sp.hasChildren);
                cont = sp.Next(true);
            }

            if (SavedFoldout(name, i))
            {
                EditorGUI.indentLevel++;
                {
                    SerializedProperty prop = soManager.FindProperty(string.Format("AudioLayerSettings.Array.data[{0}]", i));
                    prop.Next(true);
                    prop.Next(true);
                    //Debug.Log(prop.name);
                    EditorGUILayout.PropertyField(prop);
                    prop.Next(true);
                    EditorGUILayout.PropertyField(prop);
                    prop.Next(true);
                    EditorGUILayout.PropertyField(prop);
                    
                }
                EditorGUI.indentLevel--;
            }
        }
        soManager.ApplyModifiedProperties();
        
    }
}
