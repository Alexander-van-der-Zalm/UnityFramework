using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.FSM;

namespace FSM.Examples
{
    [System.Serializable]
    public class State_Airborn : State<MyStateMachine, MyVariables>
    {
        public override string GetName()
        {
            return "Airborn";
        }

        public override State<MyStateMachine, MyVariables> CheckForStateChange(MyStateMachine fsm, MyVariables vars)
        {
            if (vars.Grounded)
                return fsm.Grounded;

            return null;
        }
    }

}
