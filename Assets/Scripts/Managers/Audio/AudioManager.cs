using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Singleton AudioManager
[RequireComponent(typeof(AudioLayerManager))]
public class AudioManager : Singleton<AudioManager>
{
    #region fields

    // AudioSources    
    // Find by lambda:
    // - Layer (m)
    // - Clip (m)
    // - Sample (m)
    // - ID (s)
    // - ETC.
    public List<AudioSourceContainer> AudioSources { get; private set; }
    
    public AudioLayerManager AudioLayerManager;
    public AudioLibrary AudioLibrary;



    #endregion

    #region Instantiate

    public void Awake()
    {
        AudioSources = new List<AudioSourceContainer>();
    }

    #endregion

    // Functions

    #region Stop

    /// <summary>
    /// Stop all sound
    /// </summary>
    public static void Stop() { Stop(Instance.AudioSources); }
    public static void Stop(int AudioSourceID) { Stop(Instance.FindSource(AudioSourceID)); }
    public static void Stop(AudioLayer layer) { Stop(Instance.FindSources(layer)); }
    //public static void Stop(AudioSample sample) { Stop(Instance.FindSources(sample)); }
    public static void Stop(AudioClip clip) { Stop(Instance.FindSources(clip)); }
   
    public static void Stop(List<AudioSourceContainer> sources) 
    { 
        foreach(AudioSourceContainer source in sources)
        {
            source.AudioSource.Stop();
        }
    }
    public static void Stop(AudioSource source) { source.Stop(); }
    public static void Stop(AudioSourceContainer source) { source.AudioSource.Stop(); }

    #endregion

    #region Play

    // Three types:
    // Transform based (3D Sound)
    // Point or Vector3 Based (3D Sound)
    // None (Directly to the main audio source (2D sound)
    
    
    /// <summary>
    /// Plays directly at the cameras audioSource
    /// </summary>
    /// <param name="sample"></param>
    public static void Play(AudioSample sample)
    {
        // There should only be one audioListener (Usually main camera)
        Transform audioListenerTransform = GameObject.FindObjectOfType<AudioListener>().transform;
        Play(sample, audioListenerTransform);
    }

    /// <summary>
    /// Parents to the transform (hence follows it)
    /// </summary>
    /// <param name="sample"></param>
    /// <param name="transform"></param>
    public static void Play(AudioSample sample, Transform transform)
    {
        GameObject soundObject = RegisterAndCreateAudioSourceContainer(sample);
        soundObject.transform.parent = transform;
        soundObject.transform.position = transform.position;

        Play(soundObject);
    }

    public static void Play(AudioSample sample, Vector3 position)
    {
        GameObject soundObject = RegisterAndCreateAudioSourceContainer(sample);
        soundObject.transform.position = position;
        
        Play(soundObject);
    }

    public static void Play(AudioClip clip, AudioLayer layer = AudioLayer.None, AudioSample.AudioSettings settings = null) 
    {
        AudioSample sample = new AudioSample();
        sample.Clip = clip;
        sample.Layer = layer;
        sample.Settings = settings;

        // There should only be one audioListener (Usually main camera)
        Transform audioListenerTransform = GameObject.FindObjectOfType<AudioListener>().transform;
        Play(sample, audioListenerTransform);
    }

    private static void Play(GameObject go)
    {
        AudioSource audioSource = go.GetComponent<AudioSource>();
        audioSource.Play();
    }
   

    #endregion

    #region Pause

    #endregion

    #region Resume

    #endregion

    #region Mute

    #endregion

    #region Seek

    #endregion

    public static void Pause() { }
    public static void Resume() { }
    public static void Seek() { }
    public static void Transition() { }
    public static void ChangeVolume() { }
    public static void Lerp(AudioSample clip1, AudioSample clip2, float t) { }

    //private 

    #region Audio Source Management

    #region Add

    private static GameObject RegisterAndCreateAudioSourceContainer(AudioSample sample)
    {
        GameObject soundObject = AudioSourceContainer.CreateContainer(sample);
        //Register
        return soundObject;
    }

    private void AddAudioSource(AudioSourceContainer source)
    {
        // Handle destroy
        // Handle limit
        StartCoroutine(AddAudioSourceCR(source));
    }

    /// <summary>
    /// Garbage collection, a coroutine that keeps on running till the object is destroyed
    /// </summary>
    private IEnumerator AddAudioSourceCR(AudioSourceContainer source)
    {
        Instance.AudioSources.Add(source);
        
        // Keep Running till it is destroyed
        while(!source.DestroyMe)
            yield return null;

        Instance.AudioSources.Remove(source);
        source = null;
    }

    #endregion

    // AudioSources    
    // Find by lambda:
    // - ID (s)
    // - Layer (m)
    // - Clip (m)
    // - Sample (m)
    private AudioSourceContainer FindSource(int AudioSourceID) { return AudioSources.Where(a => a.AudioSource.GetInstanceID() == AudioSourceID).First(); }

    private List<AudioSourceContainer> FindSources(AudioLayer layer) { return AudioSources.Where(a => a.Layer == layer).ToList(); }
    private List<AudioSourceContainer> FindSources(AudioClip clip) { return AudioSources.Where(a => a.AudioSource.clip == clip).ToList(); }
    //private List<AudioSourceContainer> FindSources(AudioSample sample) { return AudioSources.Where(a => a.Sample == sample).ToList(); }

    #endregion
}
