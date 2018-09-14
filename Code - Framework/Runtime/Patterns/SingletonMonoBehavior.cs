using UnityEngine;
using System.Collections;


[ExecuteInEditMode]
public class SingletonMonoBehavior<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;

    /**
       Returns the instance of this singleton.
    */
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
                    instance = new GameObject() { name = typeof(T).FullName }.AddComponent<T>();

                    //Debug.LogError("An instance of " + typeof(T) +
                    //   " is needed in the scene, but there is none.");
                }
            }

            return instance;
        }
    }
}
