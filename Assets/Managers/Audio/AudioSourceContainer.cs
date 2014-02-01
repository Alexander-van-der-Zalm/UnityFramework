using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class AudioSourceContainer // : MonoBehaviour
{
    #region Fields

    public string Name{get;set;}
    [HideInInspector]
    public bool DestroyMe = false;

    public AudioSource AudioSource { get; set; }
    
    public AudioLayer Layer { get; set; }
    [RangeAttribute(0, 1)]
    public float VolumeModifier = 1;

    #endregion

    #region Constructors

    public AudioSourceContainer()
    {

    }

    public AudioSourceContainer(AudioSample sample)
    {
        SetSettingsFromSample(sample);
    }

    public void SetSettingsFromSample(AudioSample sample)
    {
        if (AudioSource == null)
            throw new Exception("AudioSourceContainer: No AudioSource set");
        
        VolumeModifier              = sample.Settings.Volume;

        AudioSource.loop            = sample.Settings.Loop;
        AudioSource.priority        = sample.Settings.Priority;
        AudioSource.pitch           = sample.Settings.Pitch;

        AudioSource.bypassEffects   = sample.Settings.BypassEffects;
        AudioSource.bypassListenerEffects = sample.Settings.BypassListenerEffects;
        AudioSource.bypassReverbZones = sample.Settings.BypassReverbZones;

        AudioSource.dopplerLevel    = sample.Settings.DopplerLevel;
        AudioSource.panLevel        = sample.Settings.PanLevel;
        AudioSource.spread          = sample.Settings.Spread;
        AudioSource.maxDistance     = sample.Settings.MaxDistance;

        AudioSource.pan             = sample.Settings.Pan2D;
        
    }

    #endregion

    public void Destroy()
    {
        DestroyMe = true;
        GameObject.DestroyImmediate(AudioSource);
    }
}
