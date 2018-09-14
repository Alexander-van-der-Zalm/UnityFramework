using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LoopUtilities
{
    public static void Times(this int count, Action action)
    {
        for (int i = 0; i < count; i++)
        {
            action();
        }
    }
}