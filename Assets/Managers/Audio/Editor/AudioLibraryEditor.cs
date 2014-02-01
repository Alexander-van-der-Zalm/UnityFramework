using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(AudioLibrary))]
public class AudioLibraryEditor : Editor 
{
    //[SerializeField]
    //AudioLibraryEditorData data = new AudioLibraryEditorData();

    //public void Awake()
    //{
    //    if (data == null)
    //        data = ScriptableObject.CreateInstance<AudioLibraryEditorData>();
    //    Debug.Log("Awake");
    //}

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        #region Get Data

        // Grab the object from the target in inspector
        AudioLibrary AudioLib = target as AudioLibrary;
        AudioLibraryEditorData data = AudioLib.Data;
        int samplesCount = AudioLib.Samples.Count;

        #endregion

        #region Check if settingsbools are null and the right size

        //if (data.foldSettings == null)
        //    data.foldSettings = new List<bool>(samplesCount);
        //if (data._3DSettings == null)
        //    data._3DSettings = new List<bool>(samplesCount);
        //if (data._2DSettings == null)
        //    data._2DSettings = new List<bool>(samplesCount);

        while (data.foldSettings.Count < samplesCount)
            data.foldSettings.Add(false);
        while (data._3DSettings.Count < samplesCount)
            data._3DSettings.Add(false);
        while (data._2DSettings.Count < samplesCount)
            data._2DSettings.Add(false);
        #endregion

        #region First part of the editorGui Count & Add

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Samples in library: " + samplesCount);
            if (GUILayout.Button("Add", data.addButtonStyle))
            {
                AddClipToList(data, AudioLib, null);
            }
        } EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Folder"))
                data.folderPath = EditorUtility.OpenFolderPanel("Select AudioFiles Folder", data.folderPath, "");

            string dir = "";
            if (data.folderPath != "")
                dir = FileUtility.AssetsRelativePath(data.folderPath);
            EditorGUILayout.LabelField("Dir: " + dir);

        } EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Add from directory: ");

            if (GUILayout.Button("AddFromDirectory"))
            {
                DirectoryInfo dir = new DirectoryInfo(data.folderPath);
                FileInfo[] allFiles = dir.GetFiles(); //FileUtility.GetResourcesDirectories();//data.folderPath);
                Debug.Log(allFiles.Length);
                //
                foreach (FileInfo f in allFiles)
                {
                    if (f.FullName.EndsWith(".wav") || f.FullName.EndsWith(".ogg"))
                    {
                        Debug.Log(f.FullName);

                        WWW www = new WWW(f.FullName);

                        AudioClip clip = www.GetAudioClip(true, false);
                        string[] split = f.FullName.Split('/');
                        clip.name = split[split.Length - 1];
                        if (AudioLib.Samples.Where(a => a.Clip == clip).Count() == 0)
                            AddClipToList(data, AudioLib, clip);
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
                RemoveSample(data, AudioLib, i);
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
            
            Rect hor = EditorGUILayout.BeginHorizontal();
            {
                //foldSettings[i] = EditorGUILayout.Foldout(foldSettings[i], "", settingsToggleStyle);
                Rect rect = new Rect(hor.xMin, hor.yMin, 15, hor.height);
                data.foldSettings[i] = EditorGUI.Foldout(rect, data.foldSettings[i], "", true);//, new GUIStyle() { alignment = TextAnchor.LowerRight });//,GUILayout.Width(200));

                sample.Clip = EditorGUILayout.ObjectField("", sample.Clip, typeof(AudioClip), false, data.audioClipStyle) as AudioClip;
                sample.Layer = (AudioLayer)EditorGUILayout.EnumPopup("", sample.Layer, data.audioLayerStyle);
                
                // Delete button
                if (GUILayout.Button("X", data.delButtonStyle))
                    if (EditorUtility.DisplayDialog("Delete AudioSample", "Are you sure you want to delete this audioSample?", "ok", "cancel"))
                        removeAt = i;        
            }
            EditorGUILayout.EndHorizontal();
            
            #endregion

            #region Settings

            if (data.foldSettings[i])
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
                data._3DSettings[i] = EditorGUILayout.Foldout(data._3DSettings[i], "3D Settings");
                if (data._3DSettings[i])
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
                data._2DSettings[i] = EditorGUILayout.Foldout(data._2DSettings[i], "2D Settings");
                if (data._2DSettings[i])
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
            RemoveSample(data, AudioLib, removeAt);
        }

        #endregion

        //EditorUtility.SetDirty(data);
        //UnityEditor.SerializedObject test = new SerializedObject(AudioLib);
        //UnityEditor.SerializedProperty henk = new SerializedProperty();
        //henk.p
        //GUIContent content = new GUIContent("test","tooltip");
        //EditorGUILayout.PropertyField(test, true);//content, true,GUILayout.Width(100));

        

        //Editor.DrawPropertiesExcluding(test);
    }

    private void RemoveSample(AudioLibraryEditorData data, AudioLibrary AudioLib, int removeAt)
    {
        data.foldSettings.RemoveAt(removeAt);
        data._3DSettings.RemoveAt(removeAt);
        data._2DSettings.RemoveAt(removeAt);
        AudioLib.Samples.RemoveAt(removeAt);
    }

    private void AddClipToList(AudioLibraryEditorData data, AudioLibrary AudioLib, AudioClip audioClip)
    {
        data.foldSettings.Add(false);
        data._3DSettings.Add(false);
        data._2DSettings.Add(false);
        AudioSample sample = new AudioSample();
        sample.Clip = audioClip;
        AudioLib.Samples.Add(sample);
    }

  

    //public void OnCHanged OID

    public void OnInspectorUpdate()
    {
        this.Repaint();
    }
}
