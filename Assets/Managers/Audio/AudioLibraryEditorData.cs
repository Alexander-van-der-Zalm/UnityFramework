using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class AudioLibraryEditorData
{
    public List<bool> foldSettings = new List<bool>();
    public List<bool> _3DSettings = new List<bool>();
    public List<bool> _2DSettings = new List<bool>();
    public string folderPath = "";

    public GUILayoutOption[] audioClipStyle = new GUILayoutOption[] { GUILayout.MaxWidth(150), GUILayout.MinWidth(40) };
    public GUILayoutOption[] audioLayerStyle = new GUILayoutOption[] { GUILayout.MaxWidth(70), GUILayout.MinWidth(40) };

    public GUILayoutOption[] delButtonStyle = new GUILayoutOption[] { GUILayout.MaxWidth(30), GUILayout.MinWidth(10) };
    public GUILayoutOption[] addButtonStyle = new GUILayoutOption[] { GUILayout.MaxWidth(50), GUILayout.MinWidth(30) };
}