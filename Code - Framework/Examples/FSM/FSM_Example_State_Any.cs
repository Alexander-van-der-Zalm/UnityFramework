using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.FSM;

namespace FSM.Examples
{
    [System.Serializable]
    public class State_Any : State<MyStateMachine, MyVariables>
    {
        public override string GetName()
        {
            return "Any";
        }

        public override State<MyStateMachine, MyVariables> CheckForStateChange(MyStateMachine fsm, MyVariables vars)
        {
            if (vars.Grounded)
                return fsm.Grounded;
            else
                return fsm.Airborn;
        }
    }

}
