using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateManagerMenu 
{
    [MenuItem("CustomTools/CreateNewManager")]
    static void CreateManager()
    {
        GameObject manager = GameObject.Find("Manager");
        
        if (manager != null)
            if(EditorUtility.DisplayDialog("Delete Manager", "Are you sure you want to delete the current Manager (you will lose all settings on it).", "ok", "keep it"))
                GameObject.DestroyImmediate(manager);

        manager = new GameObject();
        manager.name = "Manager";
        manager.AddComponent<AudioManager>();
        manager.AddComponent<AudioLibrary>();
    }
	
}
