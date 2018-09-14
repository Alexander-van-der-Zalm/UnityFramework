using UnityEngine;
using UnityEditor;

namespace Framework.FSM
{
    [System.Serializable]
    public abstract class Variables
    {
        //// Add a last pressed update mechanism>??
        public virtual void UpdateAnimations() { }
    }
}