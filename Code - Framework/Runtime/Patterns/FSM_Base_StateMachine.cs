using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.FSM
{
    /// <summary>
    /// Generic Template for FSM handling
    /// </summary>
    [System.Serializable]
    public abstract class StateMachine<FSM, Variables> where FSM : StateMachine<FSM, Variables> where Variables : Framework.FSM.Variables
    {
        #region Fields

        [EditorReadOnly]
        public string StateName;
        public bool DebugStateChange = false;

        private State<FSM, Variables> m_currentState;

        #endregion

        // Change to ctor?
        public void Init(State<FSM, Variables> InitialState, Variables variables)
        {
            SetState(InitialState, variables);
        }

        public void CheckForStateChange(Variables variables)
        {
            State<FSM, Variables> newState = m_currentState.CheckForStateChange((FSM)this, variables);

            if (newState != null)
            {
                SetState(newState, variables);
            }
        }

        public void Update(Variables vars)
        {
            // For now cannot change state through return
            m_currentState.Update((FSM)this, vars);
        }

        public void SetState(State<FSM, Variables> newState, Variables variables)
        {
            // Dont change state when the newstate is null
            if (newState == null)
                return;

            // Dont change state when the newstate is the current state
            if (newState == m_currentState)
                return;

            if (DebugStateChange)
                Debug.Log((m_currentState != null ? m_currentState.Name : "") + " -> " + newState.Name);

            // Call OnExit for last state
            if (m_currentState != null)
                m_currentState.OnExit(variables);

            // Call OnEnter for new State
            if (newState != null)
                newState.OnEnter(variables);

            m_currentState = newState;
            StateName = m_currentState.Name;
        }
    }
}






