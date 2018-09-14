using UnityEngine;
using System.Collections;
using System;

public class Shake : MonoBehaviour
{
    public ShakeRecipe recipe;

    private Vector3 originalPos;
    private bool shaking = false;
    
    public void StartShake()
    {
        StartShake(recipe, transform);
    }

    public void StartShake(ShakeRecipe recipe)
    {
        StartShake(recipe, transform);
    }

    public void StartShake(ShakeRecipe recipe, Transform tr)
    {
        if (!shaking)
            originalPos = transform.position;
        else
            StopAllCoroutines();

        StartCoroutine(ShakeCR(recipe, tr));
    }

    private IEnumerator ShakeCR(ShakeRecipe recipe, Transform tr)
    {
        float timepassed = 0;
        float x = UnityEngine.Random.Range(0, 1000);
        float y = UnityEngine.Random.Range(0, 1000);
        float d = 0; //Use to store sampling speed
        //Vector3 originalPos = transform.position;

        // Start shaking! :D :3
        shaking = true;

        while (timepassed < recipe.Duration)
        {
            float t = timepassed / recipe.Duration; // Range 0 - 1
            float amplitude = recipe.Amplitude.Evaluate(t);
            float samplespeed = recipe.Frequency.Evaluate(t);
            
            //ScreenshakeMAGIC
            d += timepassed * samplespeed;
            float xOffset = PerlinSample(x + d, y) * amplitude * recipe.ShakeRange;
            float yOffset = PerlinSample(x, y + d) * amplitude * recipe.ShakeRange;

            //Check and Move the object
            Move(tr, xOffset, yOffset, originalPos);

            //PassedTime and frameskip  
            timepassed += Time.deltaTime;
            yield return null;
        }

        shaking = false;
    }

    private void Move(Transform tr, float xOffset, float yOffset, Vector3 originalPos)
    {
        Rigidbody2D rb2d = tr.GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.position = new Vector2(originalPos.x + xOffset, originalPos.y + yOffset);
            return;
        }

        Rigidbody rb = tr.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.position = originalPos + new Vector3(xOffset, yOffset);
            return;
        }

        tr.transform.position = originalPos + new Vector3(xOffset, yOffset);
    }

    private float PerlinSample(float x, float y)
    {
        return (Mathf.PerlinNoise(x, y)*2-1);
    }
}
