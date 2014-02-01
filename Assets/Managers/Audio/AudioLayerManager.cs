using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

// Add all enums here
public enum AudioLayer
{   
    None,
    BGM,
    SFX,
    Dialogue
    // etc.
}

[ExecuteInEditMode]
public class AudioLayerManager : MonoBehaviour 
{
    public List<AudioLayerSettings> AudioLayerSettings= new List<AudioLayerSettings>();
    //public List<bool> foldSettings

    public void Update()
    {
        IEnumerable<AudioLayer> values = Enum.GetValues(typeof(AudioLayer)).Cast<AudioLayer>();
        if (AudioLayerSettings.Count < values.Count())
        {
            //AudioLayerSettings = new List<AudioLayerSettings>();
            foreach(AudioLayer audioLayer in values)
                AudioLayerSettings.Add(new AudioLayerSettings(audioLayer));
        }
    }

    // Layers GUI
    // Fill & Store all the LayerSettings from enum
    
}
