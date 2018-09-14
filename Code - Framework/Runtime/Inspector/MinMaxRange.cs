using UnityEngine;
using System.Collections;

[System.Serializable]
public class MinMaxRange
{
    public float Min, Max;
    public float minLimit = 0, maxLimit = 25.0f;

    public MinMaxRange(float minLimit, float maxLimit)
    {
        SetVars(minLimit, maxLimit, minLimit, maxLimit);
    }

    public MinMaxRange(float minLimit, float maxLimit, float minValue, float maxValue)
    {
        SetVars(minLimit, maxLimit, minValue, maxValue);
    }

    private void SetVars(float minLimit, float maxLimit, float minValue, float maxValue)
    {
        this.minLimit = minLimit;
        this.maxLimit = maxLimit;

        Min = minValue;
        Max = maxValue;
    }

    //public float ClampValueInRange0to1(float value)
    //{
        
    //    return Mathf.Clamp()
    //}
}
