using System;
using System.Collections.Generic;
using UnityEngine;

// From the interwebs
// https://forum.unity.com/threads/script-execution-order-manipulation.130805/#post-1323087
public class ScriptOrder : Attribute
{
    public int order;

    public ScriptOrder(int order)
    {
        this.order = order;
    }
}
