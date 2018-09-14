#if UNITY_EDITOR
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;


public static class EditorExtensionMethods
{
    public static int GetVisibleMemberCount(this SerializedProperty prop)
    {
        int count = 0;
        prop.IterateChildrenVisibleNext(x => count++);
        return count;
    }

    public static int GetMemberCount(this SerializedProperty prop)
    {
        int count = 0;
        prop.IterateFieldsSmart(x => count++);
        return count;
    }

    public static void IterateFieldsSmart(this SerializedProperty prop, Action<SerializedProperty> action)
    {
        if (prop.hasChildren && prop.GetVisibleMemberCount() == 0)
        {
            IterateFieldsReflection(prop, action);
        }
        else
        {
            IterateChildrenVisibleNext(prop, action);
        }
    }

    public static void IterateChildrenVisibleNext(this SerializedProperty prop, Action<SerializedProperty> action)
    {
        SerializedProperty copy = prop.Copy();
        string path = prop.propertyPath;
        while (copy.NextVisible(true) && copy.IsMemberOf(path))
        {
            action(copy);
        }
    }

    public static void IterateFieldsReflection(this SerializedProperty prop, Action<SerializedProperty> action)
    {
        Type type = prop.GetTypeReflection();
        var fields = type.GetFields();
        foreach (var item in fields)
        {
            action(prop.FindPropertyRelative(item.Name));
        }
    }

    public static Type GetTypeReflection(this SerializedProperty prop)
    {
        // The parent object should have a field with a type that matches propety.type
        Type parentType = prop.serializedObject.targetObject.GetType();
        var fields = parentType.GetFields();
        Type result = null;
        for (int i = 0; i < fields.Length; i++)
        {
            if(fields[i].FieldType.IsGenericType)       // Check if the type is hidden as a generic argument ex: List<mytype>
            {
                var genericArgs = fields[i].FieldType.GetGenericArguments();
                for (int j = 0; j < genericArgs.Length; j++)
                {
                    var genericArg = genericArgs[j];
                    if (prop.IsType(genericArgs[j]))
                        return genericArgs[j];
                }
            }
            else if (prop.IsType(fields[i].FieldType))
                return fields[i].FieldType;
        }
        return result;
    }

    private static bool IsType(this SerializedProperty prop, Type type)
    {
        return prop.type == type.ToString();
    }

    public static bool IsMemberOf(this SerializedProperty prop, string parentPath)
    {
        return prop.propertyPath.Split('.')[0] == parentPath;
    }

    public static void LogInfo(this SerializedProperty prop)
    {
        Debug.Log(string.Format("{0} {1} {2} {3} Children: {4} {5} {6} Array: {7}", 
            prop.name, prop.propertyPath, prop.type, prop.GetType(), prop.hasChildren, 
            prop.GetVisibleMemberCount(), prop.GetMemberCount(), prop.isArray));
    }
}

#endif