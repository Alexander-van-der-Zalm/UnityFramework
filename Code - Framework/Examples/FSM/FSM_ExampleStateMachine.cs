using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.FSM;

namespace FSM.Examples
{
    [System.Serializable]
    public class MyStateMachine : StateMachine<MyStateMachine, MyVariables>
    {
        public State_Any Any;
        public State_Airborn Airborn;
        public State_Grounded Grounded;
    }
}
