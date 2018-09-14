using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Examples
//OnFull -> delagate callback on changed
//OnEmpty -> delagate callback on changed
//OnNotFull -> delagate consistently called upon a certain condition (maybe start a coroutine that calls the delegate)
//OnNotEmpty -> delagate  consistently called upon a certain condition

// Refactoring -> Generic & seperate the value part from the minmax clamp part
// ConditionalDelegatedValue<T>
// ConditionalMinMax<T> : ConditionalDelegatedValue<T>
// IntResource :  ConditionalMinMax<int>
// FloatResource :  ConditionalMinMax<float>

// Add more conditions:
// OnSpecificValue (args?)
// OnRandomValue   (args?)
// NotEmpty 
// NotFull

    // How to execute constant conditions/states?
    // Should this class do that even?
[System.Serializable]
public class Resource
{
    #region Types
    public enum ValueChangedEnum
    {
        valueChanged = 1 << 0,
        minChanged = 1 << 1,
        maxChanged = 1 << 2,
        none = 0
    }

    public delegate void Method();
    public delegate bool Condition(Resource resource);

    #endregion

    #region Fields

    [SerializeField]
    private float value;
    [SerializeField]
    private float min;
    [SerializeField]
    private float max;

    public ValueChangedEnum ValuesChanged;

    private int callbackDepth = 0;

    private Dictionary<Condition, Method> conditionalMethods;
    private Dictionary<Condition, List<Method>> methodsLookup;

    #endregion

    #region Properties

    public float Value
    {
        get { return value; }
        set
        {
            if (this.value == value)
                return;
            this.value = value;
            ValuesChanged |= ValueChangedEnum.valueChanged;
            ClampAndCallbacks(this);
        }
    }
    public float Min
    {
        get { return min; }
        set
        {
            if (min == value)
                return;
            min = value;
            ValuesChanged |= ValueChangedEnum.minChanged;
            ClampAndCallbacks(this);
        }
    }
    public float Max
    {
        get { return max; }
        set
        {
            if (max == value)
                return;
            max = value;
            ValuesChanged |= ValueChangedEnum.maxChanged;
            ClampAndCallbacks(this);
        }
    }

    #endregion

    #region Ctor

    public Resource(float min, float max, float value)
    {
        this.min = min;
        this.max = max;
        this.value = value;
        
        ValuesChanged = ValueChangedEnum.none;

        conditionalMethods = new Dictionary<Condition, Method>();
        methodsLookup = new Dictionary<Condition, List<Method>>();
    }

    #endregion

    // Example Set
    // IsEmpty (Condition) (2x added)
    //  RegisterDeath
    //  PlayAnimation (2x)
    // IsFull (Condition) (2x)
    //  PlayAnimation (2x)
    //  SomeOtherFoo
    // 
    // IsEmpty - RegisterDeath
    // IsEmpty - PlayAnimation
    // IsFull - PlayAnimation
    // IsFull - SomeOtherFoo

    public void AddOnValueChanged(Condition condition, Method method)
    {
        List<Method> methodList;
        if (methodsLookup.TryGetValue(condition, out methodList))
        {
            if (methodList.Contains(method))  
            {
                Debug.Log("AddConditionAndMethod: Condition Method pair already in there");
                return;
            }
        }
        else
        {
            methodsLookup.Add(condition, new List<Method>());
        }

        // Add to the lookup & invoke dictionaries
        methodsLookup[condition].Add(method);

        if (conditionalMethods.ContainsKey(condition))
            conditionalMethods[condition] += method;
        else
            conditionalMethods.Add(condition, method);
    }

    public static string PrintConditionsAndMethods(Resource r)
    {
        string result = "Resource Conditions and Methods: \n";
        int i = 1;
        int j = 1;
        foreach(Condition c in r.conditionalMethods.Keys)
        {
            j = 1;
            foreach (Method m in r.methodsLookup[c])
            {
                result += c.Method.Name + " - " + m.Method.Name;
                if(j != r.methodsLookup[c].Count)
                    result += "\n";
                j++;
            }
            
            if (i != r.conditionalMethods.Count)
                result += "\n";
            i++;
        }

        return result;
    }

    private static void ClampAndCallbacks(Resource r)
    {
        if (r.callbackDepth >= 7) // Prevent stack overflow for nested and looping callbacks 
        {
            Debug.LogError("Callback on Resource has reached a depth of 7 - preventing overflow");
            return;
        }
            
        List<Condition> executeConditions = new List<Condition>(); // To prevent nested callbacks checking conditions out of order
        r.callbackDepth++;
        foreach (Condition c in r.conditionalMethods.Keys)
        {
            if (c(r))                               // Check the condition
                executeConditions.Add(c);           // Add to buffer
        }
        
        foreach(Condition c in executeConditions)
        {
            r.conditionalMethods[c]();              // Execute its callback delegate
        }
        r.callbackDepth--;

        r.value = Mathf.Clamp(r.value, r.min, r.max);   // Clamp
        r.ValuesChanged = ValueChangedEnum.none;
    }

    #region Math Operators

    public static Resource operator +(Resource r, float i)
    {
        r.Value += i;
        //ClampAndCallbacks(r);
        return r;
    }

    public static Resource operator -(Resource r, float i)
    {
        r.Value -= i;
        //ClampAndCallbacks(r);
        return r;
    }

    public static Resource operator *(Resource r, float i)
    {
        r.Value *= i;
        //ClampAndCallbacks(r);
        return r;
    }

    public static Resource operator /(Resource r, float i)
    {
        r.Value /= i;
        //ClampAndCallbacks(r);
        return r;
    }

    public static Resource operator ++(Resource r)
    {
        r.Value++;
        return r;
    }

    public static Resource operator --(Resource r)
    {
        r.Value--;
        return r;
    }

    #endregion

    #region Conditions

    public static Condition OnFull()
    {
        return r => r.value >= r.max;
    }

    public static Condition OnEmpty()
    {
        return r => r.value <= r.min;
    }

    public static Condition OnValueChanged()
    {
        return r => true;
    }

    #endregion

    public override string ToString()
    {
        return value.ToString();
    }
}