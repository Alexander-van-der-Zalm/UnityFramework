#region Using
using UnityEngine;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

#if UNITY_STANDALONE || UNITY_EDITOR
using XInputDotNetPure;
#endif

#endregion

namespace Framework.Input
{


    [System.Serializable]
    public class InputAxisKey
    {
        #region Enums

        public enum AxisKeyType
        {
            PC,
            Dpad,
            Axis
        }

        #endregion

        #region Fields

        public AxisKeyType Type;
        [EditorReadOnly]
        public string[] keys;

        //[SerializeField,HideInInspector]
        //private int selectedIndex1;
        //[SerializeField, HideInInspector]
        //private int selectedIndex2;

        #endregion

        public InputAxisKey()
        {
            keys = new string[2];
        }

        #region Creates

        public static InputAxisKey XboxAxis(XboxAxis axis)
        {
            InputAxisKey ak = new InputAxisKey();
            ak.Type = AxisKeyType.Axis;
            ak.keys[0] = axis.ToString();

            //ak.changed();

            return ak;
        }

        public static InputAxisKey XboxDpad(DirectionInput horintalOrVertical)
        {
            InputAxisKey ak = new InputAxisKey();
            ak.Type = AxisKeyType.Dpad;

            if (horintalOrVertical == DirectionInput.Horizontal)
            {
                ak.keys[0] = XboxButton.Left.ToString();
                ak.keys[1] = XboxButton.Right.ToString();
            }
            else
            {
                ak.keys[0] = XboxButton.Down.ToString();
                ak.keys[1] = XboxButton.Up.ToString();
            }

            //ak.changed();

            return ak;
        }

        public static InputAxisKey PC(string neg, string pos)
        {
            InputAxisKey ak = new InputAxisKey();
            ak.Type = AxisKeyType.PC;

            ak.keys[0] = neg;
            ak.keys[1] = pos;

            //ak.changed();

            return ak;
        }

        public static InputAxisKey PC(KeyCode neg, KeyCode pos)
        {
            return PC(neg.ToString(), pos.ToString());
        }

        #endregion

        #region Value

        public float Value(int xboxController = 0)
        {
            float v = 0;
            switch (Type)
            {
                case AxisKeyType.PC:
                    if (UnityEngine.Input.GetKey(InputHelper.ReturnKeyCode(keys[0])))
                        v--;
                    if (UnityEngine.Input.GetKey(InputHelper.ReturnKeyCode(keys[1])))
                        v++;
                    break;

                case AxisKeyType.Axis:
#if UNITY_STANDALONE || UNITY_EDITOR
                    v = XboxControllerState.Axis(InputHelper.ReturnXboxAxis(keys[0]), (PlayerIndex)xboxController);
                    break;
#endif
                case AxisKeyType.Dpad:
#if UNITY_STANDALONE || UNITY_EDITOR
                    if (XboxControllerState.ButtonDown(InputHelper.ReturnXboxButton(keys[0]), (PlayerIndex)xboxController))
                        v--;
                    if (XboxControllerState.ButtonDown(InputHelper.ReturnXboxButton(keys[1]), (PlayerIndex)xboxController))
                        v++;
#endif
                    break;

                default:
                    return 0;
            }
            return v;
        }

        #endregion

        public override string ToString()
        {
            return Type.ToString() + " " + keys[0].ToString() + " " + keys[1].ToString();
        }
    }
}
