using UnityEngine;
using System.Collections;
using UnityEditor;

public class EditorPlus : Editor
{
    #region SavedFoldout

    /// <summary>
    /// Uses EditorPrefs to save and load fold boolean data
    /// </summary>
    internal bool SavedFoldout(GUIContent name, int index = 0)
    {
        string uniqueString = "Fold: " + target.GetInstanceID().ToString() + " - " + index;
        bool fold = EditorPrefs.GetBool(uniqueString, false);
        fold = EditorGUILayout.Foldout(fold, name);
        
        if (GUI.changed)
            EditorPrefs.SetBool(uniqueString, fold);
        
        return fold;
    }

    /// <summary>
    /// Uses EditorPrefs to save and load fold boolean data
    /// </summary>
    internal bool SavedFoldout(string name, int index = 0)
    {
        return SavedFoldout(new GUIContent(name, ""), index);
    }

    /// <summary>
    /// Uses EditorPrefs to save and load fold boolean data
    /// </summary>
    internal bool SavedFoldout(GUIContent name, Rect rect, int index = 0)
    {
        string uniqueString = "Fold: " + target.GetInstanceID().ToString() + " - " + index;
        bool fold = EditorPrefs.GetBool(uniqueString, false);
        fold = EditorGUI.Foldout(rect, fold, name);

        if (GUI.changed)
            EditorPrefs.SetBool(uniqueString, fold);

        return fold;
    }

    /// <summary>
    /// Uses EditorPrefs to save and load fold boolean data
    /// </summary>
    internal bool SavedFoldout(string name, Rect rect, int index = 0)
    {
        return SavedFoldout(new GUIContent(name, ""),rect, index);
    }

    #endregion
}
