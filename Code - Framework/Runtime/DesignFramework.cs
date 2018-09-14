using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesignFramework : MonoBehaviour
{
    // 


    /// <summary>
    ///  Pools - int, float, objects
    ///   Triggers:
    ///    - OnValueChanged (int, float)
    ///    - OnAdd/Remove (objects)
    ///    - OnCollisionEnter/Exit
    ///    - State changes (FSM)
    ///    
    /// Conditional
    /// 
    /// A card (object) enters (is added to) the battlefield (object pool)
    /// When player moves do damage to x
    /// </summary>
    public DesignFramework()
    {

    }

}

public interface IConditionalDelegate<T>
{
    /// <summary>
    /// Add a condition delegate pair. The delegate only gets triggered when the condition is true.
    /// </summary>
    /// <param name="condition">Trigger of the delegate. </param>
    /// <param name="method">The logic of the delegate. </param>
    void AddConditionalDelegate(Func<bool, T> condition, Action<T> method);
    void RemoveConditionalDelegate(Func<bool, T> condition, Action<T> method);
    void PrintConditionalDelegates();
    void ClearConditonalDelegates();
}

public abstract class ConditionalDelegateBase<T> : IConditionalDelegate<T>
{
    private Dictionary<Func<bool, T>, List<Action<T>>> methodsLookup;

    public void AddConditionalDelegate(Func<bool, T> condition, Action<T> method)
    {
        throw new NotImplementedException();
    }

    public void ClearConditonalDelegates()
    {
        throw new NotImplementedException();
    }

    public void PrintConditionalDelegates()
    {
        throw new NotImplementedException();
    }

    public void RemoveConditionalDelegate(Func<bool, T> condition, Action<T> method)
    {
        throw new NotImplementedException();
    }
}

//public Observed