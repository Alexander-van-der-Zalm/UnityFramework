using System.Collections;
using System.Collections.Generic;
using Framework.FSM;
using FSM.Examples;
using UnityEngine;


public class FSM_Example_Controller : Controller_FixedUpdate<MyStateMachine, MyVariables>
{
    protected override State<MyStateMachine, FSM.Examples.MyVariables> DefaultState { get { return StateMachine.Any; } }
}

