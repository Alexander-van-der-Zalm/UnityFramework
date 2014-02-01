using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AudioLibrary : MonoBehaviour 
{
    // AudioSamples
   // [//SerializeField]
    public List<AudioSample> Samples = new List<AudioSample>(1); //{ get; set; }
    //[SerializeField]
    public AudioLibraryEditorData Data = new AudioLibraryEditorData();
}
