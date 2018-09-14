using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Framework.Input
{
    [System.Serializable]
    public class InputAxis
    {
        #region Fields

        public string Name;
        public List<InputAxisKey> AxisKeys;

        [SerializeField]
        private int lastAxis;
        [SerializeField, EditorReadOnly]
        private ControlType lastInputType = ControlType.PC;
        [SerializeField]
        private int xbox;

        public int PlayerIndex { get { return xbox; } set { xbox = value; } }
        public ControlType LastInputType { get { return lastInputType; } }

        #endregion

        public InputAxis(int xbox = 0, string name = "defaultAxis")
        {
            AxisKeys = new List<InputAxisKey>();
            this.xbox = 0;
            this.Name = name;
        }

        public float Value()
        {
            // Check if the last active axis input is still active
            float lastAxisValue = AxisKeys[lastAxis].Value(xbox);
            if (lastAxisValue != 0)
                return lastAxisValue;

            // Check for the rest of the axis for a new input
            for (int i = 0; i < AxisKeys.Count; i++)
            {
                if (i == lastAxis)
                    continue;

                float currentValue = AxisKeys[i].Value(xbox);

                if (currentValue != 0)
                {
                    lastAxis = i;
                    lastInputType = InputHelper.AxisKeyToControl(AxisKeys[i].Type);
                    return currentValue;
                }
            }

            //Debug.Log(AxisKeys[lastAxis] + " " + value);
            return 0;
        }

        #region Creates

        public void XboxAxis(XboxAxis axis)
        {
            AxisKeys.Add(InputAxisKey.XboxAxis(axis));
        }

        public void XboxDpad(DirectionInput horizontalOrVertical)
        {
            AxisKeys.Add(InputAxisKey.XboxDpad(horizontalOrVertical));
        }

        public void PC(string neg, string pos)
        {
            AxisKeys.Add(InputAxisKey.PC(neg, pos));
        }

        public void PC(KeyCode neg, KeyCode pos)
        {
            AxisKeys.Add(InputAxisKey.PC(neg, pos));
        }

        public void DefaultInput(DirectionInput horintalOrVertical, int xbox = 0, string name = "")
        {
            switch (horintalOrVertical)
            {
                case DirectionInput.Horizontal:
                    PC("A", "D");
                    PC(KeyCode.LeftArrow, KeyCode.RightArrow);
                    XboxAxis(Framework.Input.XboxAxis.LeftX);
                    XboxDpad(DirectionInput.Horizontal);
                    break;
                case DirectionInput.Vertical:
                    PC("S", "W");
                    PC(KeyCode.DownArrow, KeyCode.UpArrow);
                    XboxAxis(Framework.Input.XboxAxis.LeftY);
                    XboxDpad(DirectionInput.Vertical);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Setup wasd, arrows, dpad, thumbstick
        /// </summary>
        /// <param name="horintalOrVertical"></param>
        /// <param name="xbox"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static InputAxis Default(DirectionInput horintalOrVertical, int xbox = 0, string name = "")
        {
            InputAxis ax = new InputAxis(xbox, name);
            ax.DefaultInput(horintalOrVertical, xbox, name);
            return ax;
        }



        #endregion
    }
}
