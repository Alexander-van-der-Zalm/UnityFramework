using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class ShakeRecipe : ScriptableObject
{
    public string Name = "Default";
    public float Duration = 1f;
    public float ShakeRange = 1f;
    public AnimationCurve Amplitude = AnimationCurve.EaseInOut(0, 1, 1, 0);
    public AnimationCurve Frequency = AnimationCurve.Linear(0, 1, 1, 1);
}
