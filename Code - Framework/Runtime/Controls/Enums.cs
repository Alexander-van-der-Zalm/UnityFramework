using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Input
{
    public enum DirectionInput
    {
        Horizontal,
        Vertical
    }

    public enum ControlType
    {
        PC,
        Xbox
    }

    public enum XboxButton
    {
        A, B, X, Y, Up, Down, Left, Right, RightShoulder, LeftShoulder, LeftStick, RightStick, Back, Start, Guide, None
    }

    public enum XboxAxis
    {
        LeftX, LeftY, RightX, RightY, LeftTrigger, RightTrigger
    }

    public enum XboxDPad
    {
        Up, Down, Left, Right
    }

    public enum UpdateTypeE
    {
        Update,
        FixedUpdate,
        LateUpdate
    }

}
