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
[Serializable]
public class AudioLayerManager : Singleton<AudioLayerManager> 
{
    [SerializeField]
    private List<AudioLayerSettings> audioLayerSettings= new List<AudioLayerSettings>();

    public void Start()
    {
        // Possibly buggy
        // Instantiates the list based on the enum
        IEnumerable<AudioLayer> values = Enum.GetValues(typeof(AudioLayer)).Cast<AudioLayer>();
        if (audioLayerSettings.Count < values.Count())
        {
            foreach(AudioLayer audioLayer in values)
                audioLayerSettings.Add(new AudioLayerSettings(audioLayer));
        }
    }

    public static AudioLayerSettings GetAudioLayerSettings(AudioLayer layer)
    {
        return Instance.audioLayerSettings.Where(a => a.Layer == layer).First();
    }
}
