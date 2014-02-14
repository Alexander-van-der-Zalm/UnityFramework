using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class AudioSourceContainer  : MonoBehaviour
{
    #region Fields

    public string Name;
    
    [HideInInspector]
    public bool DestroyMe = false;

    // Changed = new volume
    public AudioSource AudioSource { get { return audioSource; } private set { audioSource = value; } } 
    [SerializeField]
    private AudioSource audioSource;

    // Changed = new volume
    public AudioLayer Layer;
    private AudioLayerSettings layerSettings { get { return AudioLayerManager.GetAudioLayerSettings(Layer); } }

    // Changed = new volume
    public float VolumeModifier { get { return volumeModifier; } set { volumeModifier = value; UpdateVolume(); } }
    [RangeAttribute(0, 1)]
    private float volumeModifier = 1;

    #endregion

    #region Constructors

    public static GameObject CreateContainer(AudioSample sample)
    {
        GameObject soundObject = new GameObject();
        soundObject.name = "AudioSourceObject";
        AudioSourceContainer container = soundObject.AddComponent<AudioSourceContainer>();
        container.AudioSource = soundObject.AddComponent<AudioSource>();

        container.SetSettingsFromSample(sample);

        return soundObject;
    }

    private void SetSettingsFromSample(AudioSample sample)
    {
        if (AudioSource == null)
            throw new Exception("AudioSourceContainer: No AudioSource set");

        Layer = sample.Layer;
        AudioLayerSettings layerS = layerSettings;

        VolumeModifier              = sample.Settings.Volume;

        AudioSource.mute            = layerS.Mute;
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

        AudioSource.clip            = sample.Clip;
    }

    #endregion

    public void UpdateVolume()
    {
        AudioSource.volume = VolumeModifier * layerSettings.Volume;
    }

    public void Destroy()
    {
        DestroyMe = true;
        GameObject.DestroyImmediate(AudioSource);
    }
}
