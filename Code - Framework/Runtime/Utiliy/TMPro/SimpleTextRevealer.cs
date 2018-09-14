using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimpleTextRevealer : MonoBehaviour
{
    public float CharactersPerSecond = 30;
    public float FastCharactersPerSecond = 100;
    public bool Fast = false;

    private TMP_Text text;
    private float visibleCharacters;
    private string oldText;

	// Use this for initialization
	void Start ()
    {
        text = GetComponent<TMP_Text>();
        oldText = text.text;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Maybe reset vis chars?
        if (text.text != oldText)
        {
            oldText = text.text;
            Debug.Log("Text changed");
        }

        if (visibleCharacters <= text.textInfo.characterCount)
        {
            visibleCharacters = Mathf.Clamp
            (
                visibleCharacters + (Fast ? FastCharactersPerSecond : CharactersPerSecond) * Time.deltaTime, 
                0, 
                text.textInfo.characterCount
            );
            text.maxVisibleCharacters = (int)visibleCharacters;
        }
	}

    [ContextMenu("Reset Counter")]
    public void ResetCounter()
    {
        visibleCharacters = 0;
    }
}
