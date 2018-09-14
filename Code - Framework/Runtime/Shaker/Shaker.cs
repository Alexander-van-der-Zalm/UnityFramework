using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : SingletonMonoBehavior<Shaker>
{
    public static Vector3 ShakeOffset3D { get { return new Vector3(shakeOffset.x,shakeOffset.y); }}
    public static Vector2 ShakeOffset2D { get { return shakeOffset; } }

    private static Vector2 shakeOffset;

    public static void StartShake(ShakeRecipe recipe)
    {
        Shaker.Instance.StopAllCoroutines();

        Shaker.Instance.StartCoroutine(Shaker.Instance.ShakeCR(recipe));
    }

      
    private IEnumerator ShakeCR(ShakeRecipe recipe)
    {
        float timepassed = 0;
        float x = UnityEngine.Random.Range(0, 1000);
        float y = UnityEngine.Random.Range(0, 1000);
        float d = 0; //Use to store sampling speed

        // Start shaking! :D :3
        while (timepassed < recipe.Duration)
        {
            float t = timepassed / recipe.Duration; // Range 0 - 1
            float amplitude = recipe.Amplitude.Evaluate(t);
            float samplespeed = recipe.Frequency.Evaluate(t);

            //ScreenshakeMAGIC
            d += timepassed * samplespeed;
            shakeOffset.x = PerlinSample(x + d, y) * amplitude * recipe.ShakeRange;
            shakeOffset.y = PerlinSample(x, y + d) * amplitude * recipe.ShakeRange;

            //PassedTime and frameskip  
            timepassed += Time.deltaTime;
            yield return null;
        }
    }

    private float PerlinSample(float x, float y)
    {
        return (Mathf.PerlinNoise(x, y) * 2 - 1);
    }
}
