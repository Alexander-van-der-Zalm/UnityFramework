using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.FSM;

namespace FSM.Examples
{
    [System.Serializable]
    public class State_Grounded : State<MyStateMachine, MyVariables>
    {
        public override string GetName()
        {
            return "Grounded";
        }

        public override State<MyStateMachine, MyVariables> CheckForStateChange(MyStateMachine fsm, MyVariables vars)
        {
            if (!vars.Grounded)
                return fsm.Airborn;

            return null;

            // Example using any state
            //return fsm.Any.CheckForStateChange(fsm, vars);
        }

        public override void Update(MyStateMachine fsm, MyVariables vars)
        {
            // Handle logic here
        }
    }

}
