using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Singleton<T> where T : new()
{
    protected static T instanceInternal;

    /**
       Returns the instance of this singleton.
    */
    public static T Instance
    {
        get
        {
            if (instanceInternal == null)
            {
                instanceInternal = new T();
            }

            return instanceInternal;
        }
    }
}
