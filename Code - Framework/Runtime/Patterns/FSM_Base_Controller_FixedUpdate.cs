using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.FSM
{
    public abstract class Controller_FixedUpdate<FSM, Vars> : MonoBehaviour
        where FSM : StateMachine<FSM, Vars> 
        where Vars : Variables
        
    {
        public FSM StateMachine;
        public Vars Variables;

        protected abstract State<FSM,Vars> DefaultState { get; }

        protected virtual void Start()
        {
            StateMachine.Init(DefaultState, Variables);
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            StateMachine.CheckForStateChange(Variables);
        }

        protected virtual void FixedUpdate()
        {
            StateMachine.Update(Variables);
        }
    }
}

