using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class FileUtility
{

    /// <summary>
    /// Determine whether a given path is a directory.
    /// </summary>
    public static bool PathIsDirectory(string absolutePath)
    {
        FileAttributes attr = File.GetAttributes(absolutePath);
        if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            return true;
        else
            return false;
    }


    /// <summary>
    /// Given an absolute path, return a path rooted at the Assets folder.
    /// </summary>
    /// <remarks>
    /// Asset relative paths can only be used in the editor. They will break in builds.
    /// </remarks>
    /// <example>
    /// /Folder/UnityProject/Assets/resources/music returns Assets/resources/music
    /// </example>
    public static string AssetsRelativePath(string absolutePath)
    {
        if (absolutePath.StartsWith(Application.dataPath))
        {
            return "Assets" + absolutePath.Substring(Application.dataPath.Length);
        }
        else
        {
            throw new System.ArgumentException("Full path does not contain the current project's Assets folder", "absolutePath");
        }
    }


    /// <summary>
    /// Get all available Resources directory paths within the current project.
    /// </summary>
    public static string[] GetResourcesDirectories()
    {
        List<string> result = new List<string>();
        Stack<string> stack = new Stack<string>();
        // Add the root directory to the stack
        stack.Push(Application.dataPath);
        // While we have directories to process...
        while (stack.Count > 0)
        {
            // Grab a directory off the stack
            string currentDir = stack.Pop();
            try
            {
                foreach (string dir in Directory.GetDirectories(currentDir))
                {
                    if (Path.GetFileName(dir).Equals("Resources"))
                    {
                        // If one of the found directories is a Resources dir, add it to the result
                        result.Add(dir);
                    }
                    // Add directories at the current level into the stack
                    stack.Push(dir);
                }
            }
            catch
            {
                Debug.LogError("Directory " + currentDir + " couldn't be read from.");
            }
        }
        return result.ToArray();
    }
}