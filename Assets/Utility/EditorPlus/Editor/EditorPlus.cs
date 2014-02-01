using UnityEngine;
using System.Collections;
using UnityEditor;

public class EditorPlus : Editor
{
    #region SavedFoldout

    internal bool SavedFoldout(GUIContent name, int index = 0)
    {
        string uniqueString = "Fold: " + target.GetInstanceID().ToString() + " - " + index;
        bool fold = EditorPrefs.GetBool(uniqueString, false);
        fold = EditorGUILayout.Foldout(fold, name);
        
        if (GUI.changed)
            EditorPrefs.SetBool(uniqueString, fold);
        
        return fold;
    }

    internal bool SavedFoldout(string name, int index = 0)
    {
        return SavedFoldout(new GUIContent(name, ""), index);
    }

    #endregion
}
