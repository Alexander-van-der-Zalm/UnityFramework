using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class AudioSample 
{
    [Serializable]
    public class AudioSettings
    {
        [SerializeField]
        [RangeAttribute(0, 1)]
        public float Volume = 1.0f;
        [SerializeField]
        public bool Loop;
        [SerializeField]
        [RangeAttribute(0,255)]
        public int Priority = 128;
        [SerializeField]
        [RangeAttribute(-3, 3)]
        public float Pitch = 1;

        [SerializeField]
        public bool BypassEffects = false;
        [SerializeField]
        public bool BypassListenerEffects = false;
        [SerializeField]
        public bool BypassReverbZones = false;

        // 3D Sound Settings
        [SerializeField]
        [RangeAttribute(0, 5)]
        public float DopplerLevel = 1;
        [SerializeField]
        [RangeAttribute(0, 1)]
        public float PanLevel = 1;
        [SerializeField]
        [RangeAttribute(0, 360)]
        public float Spread = 0;

        [SerializeField]
        public float MaxDistance = 500;
        
        // 2D Sound Settings
        [SerializeField]
        [RangeAttribute(-1, 1)]
        public float Pan2D = 1;
        // Etc.
    }
    [SerializeField]
    public string Name = "";
    [SerializeField]
    public AudioClip Clip;
    [SerializeField]
    public AudioLayer Layer;
    [SerializeField]
    public AudioSettings Settings;
}
