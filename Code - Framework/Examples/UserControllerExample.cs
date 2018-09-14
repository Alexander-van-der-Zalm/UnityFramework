using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Input;

[RequireComponent(typeof(FSM_Example_Controller))]
public class UserControllerExample : MonoBehaviour
{
    public InputSettingsExample Input;
    private FSM_Example_Controller controller;

	void Start ()
    {
        Input.UpdatePlayerIndex();                                  // Set Player index
        controller = GetComponent<FSM_Example_Controller>();        // Get the fsm controller
	}

	void Update ()
    {
        controller.Variables.Horizontal = Input.Horizontal.Value(); // Set variables directly. Possible add a method to your controller
        controller.Variables.Vertical = Input.Vertical.Value();     // This works straight away.
        controller.Variables.Jump = new ActionCopy(Input.Jump);         // Creating an action which remembers pressed etc.

        if(Input.Jump.IsPressed())
        {
            Debug.Assert(Input.Jump.IsPressed() == controller.Variables.Jump.IsPressed);
        }
        if (Input.Jump.IsReleased())
        {
            Debug.Assert(Input.Jump.IsReleased() == controller.Variables.Jump.IsReleased);
        }
    }
}
