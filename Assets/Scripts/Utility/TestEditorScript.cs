using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TestEditorScript : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
        Debug.Log("Start");  
	}

    void Awake()
    {
        Debug.Log("Awake");
    }
	
	// Update is called once per frame
	void Update () 
    {
        Debug.Log("Update"); 
	}

    
}
