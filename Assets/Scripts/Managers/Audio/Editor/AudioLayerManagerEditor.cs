using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(AudioLayerManager))]
public class AudioLayerManagerEditor : EditorPlus
{
    private string description = "The layers are based on the hardcoded AudioLayersEnum.\n The enum can be found in the AudioLayerManager.cs file. This might change in the future.";
    private string settingsNameTooltip = "The layer names are hardcoded in the AudioLayerManager";

    public override void OnInspectorGUI()
    {
        AudioLayerManager manager = target as AudioLayerManager;
        SerializedObject soManager = new SerializedObject(manager);
        
        int count = manager.AudioLayerSettings.Count;

        if(SavedFoldout("Description"))
            EditorGUILayout.TextArea(description);
        for (int i = 0; i < count; i++)
        {
            AudioLayerSettings settings = manager.AudioLayerSettings[i]; 
            GUIContent name = new GUIContent("AudioLayer: " + settings.Layer.ToString(), settingsNameTooltip);

            if (SavedFoldout(name, i))
            {
                EditorGUI.indentLevel++;
                {
                    SerializedProperty prop = soManager.FindProperty(string.Format("AudioLayerSettings.Array.data[{0}]", i));
                    prop.Next(true);
                    prop.Next(true);
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
