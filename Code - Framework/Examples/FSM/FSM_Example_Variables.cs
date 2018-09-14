using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Input;
using Framework.FSM;

namespace FSM.Examples
{
    [System.Serializable]
    public class MyVariables : Variables
    {
        public bool Grounded;                   // Make this readonly too, but for testing purposes its left out ;)
        public ActionCopy Jump;
        [EditorReadOnly]
        public float Horizontal, Vertical;
    }
}
