using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


[RequireComponent(typeof(AudioLayerManager))]
public class AudioManager :  MonoBehaviour
{
    #region fields

    // Singleton AudioManager
    public static AudioManager Main {get; private set;}

    // AudioSources    
    // Find by lambda:
    // - Layer (m)
    // - Clip (m)
    // - Sample (m)
    // - ID (s)
    // - ETC.
    public List<AudioSourceContainer> AudioSources { get; private set; }
    
    #endregion

    #region Instantiate

    public void Awake()
    {
        Main = this;
        AudioSources = new List<AudioSourceContainer>();
    }

    #endregion

    // Functions

    #region Stop

    /// <summary>
    /// Stop all sound
    /// </summary>
    public static void Stop() { Stop(Main.AudioSources); }
    public static void Stop(int AudioSourceID) { Stop(Main.FindSource(AudioSourceID)); }
    public static void Stop(AudioLayer layer) { Stop(Main.FindSources(layer)); }
    //public static void Stop(AudioSample sample) { Stop(Main.FindSources(sample)); }
    public static void Stop(AudioClip clip) { Stop(Main.FindSources(clip)); }
   
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
    public static void Play(AudioSample sample)
    {

    }

    public static void Play(AudioSample sample, Transform transform)
    {
        AudioSourceContainer container = new AudioSourceContainer();

    }

    public static void Play(AudioClip clip) 
    { 

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
    public static void Lerp(AudioSample clip1, AudioSample clip2, float t) { }

    //private 

    #region Audio Source Management

    #region Add

    private void AddAudioSource(AudioSourceContainer source)
    {
        StartCoroutine(AddAudioSourceCR(source));
    }

    /// <summary>
    /// Garbage collection, a coroutine that keeps on running till the object is destroyed
    /// </summary>
    private IEnumerator AddAudioSourceCR(AudioSourceContainer source)
    {
        Main.AudioSources.Add(source);
        
        // Keep Running till it is destroyed
        while(!source.DestroyMe)
            yield return null;

        Main.AudioSources.Remove(source);
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
