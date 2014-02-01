using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(AudioLibrary))]
public class AudioLibraryEditor : EditorPlus
{
    #region Fields

    private GUILayoutOption[] audioClipStyle = new GUILayoutOption[] { GUILayout.MaxWidth(150), GUILayout.MinWidth(40) };
    private GUILayoutOption[] audioLayerStyle = new GUILayoutOption[] { GUILayout.MaxWidth(70), GUILayout.MinWidth(40) };

    private GUILayoutOption[] delButtonStyle = new GUILayoutOption[] { GUILayout.MaxWidth(30), GUILayout.MinWidth(10) };
    private GUILayoutOption[] addButtonStyle = new GUILayoutOption[] { GUILayout.MaxWidth(50), GUILayout.MinWidth(30) };

    private string audioLibraryFolderPath = "AudioLibraryFolderPath";

    #endregion

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        #region Get Data

        // Grab the object from the target in inspector
        AudioLibrary AudioLib = target as AudioLibrary;
//AudioLibraryEditorData data = AudioLib.Data;
        int samplesCount = AudioLib.Samples.Count;

        #endregion

        #region First part of the editorGui Count & Add

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Samples in library: " + samplesCount);
            if (GUILayout.Button("Add", addButtonStyle))
            {
                AddClipToList(AudioLib, null);
            }
        } EditorGUILayout.EndHorizontal();

        string folderPath;
        EditorGUILayout.BeginHorizontal();
        {
            // Check if there is already an dirpath in the editorPrefs
            folderPath = EditorPrefs.GetString(audioLibraryFolderPath, "");
            
            // If the button is pressed, find a new folder
            if (GUILayout.Button("Folder"))
                folderPath = EditorUtility.OpenFolderPanel("Select AudioFiles Folder", folderPath, "");

            // Show the directory in a user friendly manner
            string dir = "";
            if (folderPath != "")
                dir = FileUtility.AssetsRelativePath(folderPath);
            EditorGUILayout.LabelField("Dir: " + dir);
            
            // Save the changed string to the EditorPreference file
            if(GUI.changed)
                EditorPrefs.SetString(audioLibraryFolderPath, folderPath);

        } EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Add from directory: ");

            if (GUILayout.Button("AddFromDirectory"))
            {
                DirectoryInfo dir = new DirectoryInfo(folderPath);
                FileInfo[] allFiles = dir.GetFiles(); //FileUtility.GetResourcesDirectories();//data.folderPath);
                Debug.Log(allFiles.Length);
                //
                foreach (FileInfo f in allFiles)
                {
                    if (f.FullName.EndsWith(".wav") || f.FullName.EndsWith(".ogg"))
                    {
                        // Does not work
                        Debug.Log(f.FullName);

                        WWW www = new WWW(f.FullName);

                        AudioClip clip = www.GetAudioClip(true, false);
                        string[] split = f.FullName.Split('/');
                        clip.name = split[split.Length - 1];
                        if (AudioLib.Samples.Where(a => a.Clip == clip).Count() == 0)
                            AddClipToList(AudioLib, clip);
                        //clip
                    }
                }
                //Debug.Log(data.folderPath);
                //Debug.Log(FileUtility.AssetsRelativePath(data.folderPath));
            }
            //folderPath = EditorUtility.OpenFolderPanel("Select AudioFiles Folder", folderPath, "");

        } EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Clear"))
        {
            for (int i = samplesCount - 1; i >= 0; i--)
            {
                RemoveSample(AudioLib, i);
            }
            samplesCount = AudioLib.Samples.Count();
        }

        EditorGUILayout.Space();
        #endregion

        #region SampleLoop

        int removeAt = -1; // to store the index at where to delete at (must be bigger than 0)
        for (int i = 0; i < samplesCount; i++)
        {
            AudioSample sample = AudioLib.Samples[i];

            // Instantiate if null
            if (sample == null)
                sample = new AudioSample();

            #region Clip, Layer, Delete, Settingsfold

            bool fold;

            Rect hor = EditorGUILayout.BeginHorizontal();
            {
                Rect rect = new Rect(hor.xMin, hor.yMin, 15, hor.height);
                fold = SavedFoldout("", rect, i,"base");

                sample.Clip = EditorGUILayout.ObjectField("", sample.Clip, typeof(AudioClip), false, audioClipStyle) as AudioClip;
                sample.Layer = (AudioLayer)EditorGUILayout.EnumPopup("", sample.Layer, audioLayerStyle);
                
                // Delete button
                if (GUILayout.Button("X", delButtonStyle))
                    if (EditorUtility.DisplayDialog("Delete AudioSample", "Are you sure you want to delete this audioSample?", "ok", "cancel"))
                        removeAt = i;        
            }
            EditorGUILayout.EndHorizontal();
            
            #endregion

            #region Settings

            if (fold)
            {
                EditorGUI.indentLevel++;
                sample.Name = EditorGUILayout.TextField("Name:", sample.Name, GUILayout.Width(250));
                //[RangeAttribute(0,1)]
                sample.Settings.Volume = EditorGUILayout.Slider("Volume: ",sample.Settings.Volume,0,1);
                EditorGUILayout.Space();

                sample.Settings.Loop = EditorGUILayout.Toggle("Loop: ", sample.Settings.Loop);
                //[RangeAttribute(0,255)]
                sample.Settings.Priority = EditorGUILayout.IntSlider("Priority: ",sample.Settings.Priority,0,255);
                //[RangeAttribute(-3, 3)]
                sample.Settings.Pitch = EditorGUILayout.Slider("Pitch: ",sample.Settings.Pitch,-3,3);
               
                EditorGUILayout.Space();
                sample.Settings.BypassEffects = EditorGUILayout.Toggle("BypassEffects:",sample.Settings.BypassEffects);
                sample.Settings.BypassListenerEffects = EditorGUILayout.Toggle("BypassListenerFX:",sample.Settings.BypassListenerEffects);
                sample.Settings.BypassReverbZones = EditorGUILayout.Toggle("BypassReverbZones:",sample.Settings.BypassReverbZones);

                // 3D Sound Settings
                if (SavedFoldout("3D Settings",i,"3D"))
                {
                    EditorGUI.indentLevel++;
                    //[RangeAttribute(0, 5)]
                    sample.Settings.DopplerLevel = EditorGUILayout.Slider("DopplerLevel:",sample.Settings.DopplerLevel, 0, 5);
                    //[RangeAttribute(0, 1)]
                    sample.Settings.PanLevel = EditorGUILayout.Slider("PanLevel:",sample.Settings.PanLevel, 0, 1);
                    //[RangeAttribute(0, 360)]
                    sample.Settings.Spread = EditorGUILayout.Slider("Spread: ",sample.Settings.Spread,0,360);
                    sample.Settings.MaxDistance = EditorGUILayout.FloatField("MaxDistance",sample.Settings.MaxDistance);
                    EditorGUI.indentLevel--;
                }

                 // 2D Sound Settings
                //data._2DSettings[i] = EditorGUILayout.Foldout(data._2DSettings[i], "2D Settings");
                if (SavedFoldout("2D Settings",i,"2D"))
                {
                    EditorGUI.indentLevel++;
                    //[RangeAttribute(-1, 1)]
                    sample.Settings.Pan2D = EditorGUILayout.Slider("Pan2D:",sample.Settings.Pan2D, -1, 1);
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
            #endregion

            // Test if necessary
            //AudioLib.Samples[i] = sample;

        }

        #endregion

        #region IfDeleteSample

        // Do the deletion outside of the loop
        if (removeAt >= 0)
        {
            RemoveSample(AudioLib, removeAt);
        }

        #endregion
    }

    private void RemoveSample(AudioLibrary AudioLib, int removeAt)
    {
        //data.foldSettings.RemoveAt(removeAt);
        //data._3DSettings.RemoveAt(removeAt);
        //data._2DSettings.RemoveAt(removeAt);
        AudioLib.Samples.RemoveAt(removeAt);
        RemoveSavedFoldout("2D", removeAt);
        RemoveSavedFoldout("3D", removeAt);
        RemoveSavedFoldout("base", removeAt);
    }

    private void AddClipToList(AudioLibrary AudioLib, AudioClip audioClip)
    {
        //data.foldSettings.Add(false);
        //data._3DSettings.Add(false);
        //data._2DSettings.Add(false);
        AudioSample sample = new AudioSample();
        sample.Clip = audioClip;
        AudioLib.Samples.Add(sample);
    }

    public void OnInspectorUpdate()
    {
        this.Repaint();
    }
}
