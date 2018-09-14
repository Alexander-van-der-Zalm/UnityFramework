using UnityEngine;
using System.Collections;

namespace Framework.FSM
{
    [System.Serializable]
    public abstract class State<FSM, Variables> where FSM : StateMachine<FSM, Variables> where Variables : Framework.FSM.Variables
    {
        protected string name = "BaseState";
        public string Name { get { return name; } }
        
        public State()
        {
            name = GetName();
        }

        public abstract string GetName();

        /// <summary>
        /// Can change the state.
        /// </summary>
        /// <returns>New state to change into or null for no change.</returns>
        public virtual State<FSM, Variables> CheckForStateChange(FSM fsm, Variables vars)
        {
            return null;
        }

        // Can this one change the state too?
        /// <summary>
        /// Handle state logic here
        /// </summary>
        public virtual void Update(FSM fsm, Variables vars)
        { }

        /// <summary>
        /// Called on freshly switched to
        /// </summary>
        public virtual void OnEnter(Variables vars)
        { }


        /// <summary>
        /// Called when fsm state has been switched to something else
        /// </summary>
        public virtual void OnExit(Variables vars)
        { }
    }
}
