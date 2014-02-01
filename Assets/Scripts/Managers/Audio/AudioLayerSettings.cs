using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class AudioLayerSettings
{
    public AudioLayer Layer { get { return layer; } private set { layer = value; } }

    [SerializeField]
    private AudioLayer layer;

    [RangeAttribute(0, 1)]
    public float Volume = 1;

    public bool Mute = false;

    public int MaxClips = -1;

    public int ClipsPlaying = 0;

    public AudioLayerSettings(AudioLayer layer)
    {
        this.layer = layer;
    }

}
