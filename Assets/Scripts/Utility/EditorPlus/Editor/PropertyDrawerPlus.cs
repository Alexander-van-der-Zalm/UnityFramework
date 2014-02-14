using UnityEngine;
using System.Collections;
using UnityEditor;

public class PropertyDrawerPlus : PropertyDrawer
{
    #region SavedFoldout

    /// <summary>
    /// Uses EditorPrefs to save and load fold boolean data
    /// </summary>
    internal bool SavedFoldout(GUIContent name, int index = 0, string uniqueID = "")
    {
        string uniqueString = "Fold: " + target.GetInstanceID().ToString() + " - " + uniqueID + index;
        bool fold = EditorPrefs.GetBool(uniqueString, false);
        fold = EditorGUILayout.Foldout(fold, name);
        
        if (GUI.changed)
            EditorPrefs.SetBool(uniqueString, fold);
        
        return fold;
    }

    /// <summary>
    /// Uses EditorPrefs to save and load fold boolean data
    /// </summary>
    internal bool SavedFoldout(string name, int index = -1, string uniqueID = "")
    {
        return SavedFoldout(new GUIContent(name, ""), index, uniqueID);
    }

    /// <summary>
    /// Uses EditorPrefs to save and load fold boolean data
    /// </summary>
    internal bool SavedFoldout(GUIContent name, Rect rect, int index = 0, string uniqueID = "")
    {
        string uniqueString = "Fold: " + target.GetInstanceID().ToString() + " - " + uniqueID + index;
        bool fold = EditorPrefs.GetBool(uniqueString, false);
        fold = EditorGUI.Foldout(rect, fold, name);

        if (GUI.changed)
            EditorPrefs.SetBool(uniqueString, fold);

        return fold;
    }

    /// <summary>
    /// Uses EditorPrefs to save and load fold boolean data
    /// </summary>
    internal bool SavedFoldout(string name, Rect rect, int index = 0, string uniqueID = "")
    {
        return SavedFoldout(new GUIContent(name, ""), rect, index, uniqueID);
    }

    internal void RemoveSavedFoldout(string uniqueID, int index = 0)
    {
        string baseString = "Fold: " + target.GetInstanceID().ToString() + " - " + uniqueID;
        string curString = baseString + index;
        string nextString = baseString + ++index;
        
        while (EditorPrefs.HasKey(nextString))
        {
            bool nextBool = EditorPrefs.GetBool(nextString);
            EditorPrefs.SetBool(curString, nextBool);
            nextString = baseString + ++index;
        }
    }

    #endregion
}
