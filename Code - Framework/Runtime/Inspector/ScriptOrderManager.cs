using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
/// <summary>
/// https://forum.unity.com/threads/script-execution-order-manipulation.130805/#post-1323087
/// </summary>
[InitializeOnLoad]
public class ScriptOrderManager
{
    static ScriptOrderManager()
    {
        foreach (MonoScript monoScript in MonoImporter.GetAllRuntimeMonoScripts())
        {
            if (monoScript.GetClass() != null)
            {
                foreach (var a in Attribute.GetCustomAttributes(monoScript.GetClass(), typeof(ScriptOrder)))
                {
                    var currentOrder = MonoImporter.GetExecutionOrder(monoScript);
                    var newOrder = ((ScriptOrder)a).order;
                    if (currentOrder != newOrder)
                        MonoImporter.SetExecutionOrder(monoScript, newOrder);
                }
            }
        }
    }
}
#endif