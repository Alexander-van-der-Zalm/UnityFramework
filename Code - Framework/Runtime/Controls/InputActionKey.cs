#region Using

using UnityEngine;
using System.Collections;

#if UNITY_STANDALONE || UNITY_EDITOR
using XInputDotNetPure;
#endif

#endregion

namespace Framework.Input
{
    [System.Serializable]
    public class InputActionKey
    {
        #region Fields

        public ControlType Type;
        [EditorReadOnly]
        public string KeyValue;

        #endregion

        public InputActionKey(ControlType type = ControlType.PC, string value = "A")
        {
            Type = type;
            KeyValue = value;
        }

        #region Down,Pressed,Released

        public bool IsDown(int xbox = 0)
        {
            switch (Type)
            {
                case ControlType.PC:
                    return UnityEngine.Input.GetKey(InputHelper.ReturnKeyCode(KeyValue));
                case ControlType.Xbox:
#if UNITY_STANDALONE || UNITY_EDITOR
                    return XboxControllerState.ButtonDown(InputHelper.ReturnXboxButton(KeyValue), (PlayerIndex)xbox);
#endif
                default:
                    return false;
            }
        }

        public bool IsPressed(int xbox = 0)
        {
            switch (Type)
            {
                case ControlType.PC:
                    return UnityEngine.Input.GetKeyDown(InputHelper.ReturnKeyCode(KeyValue));
                case ControlType.Xbox:
#if UNITY_STANDALONE || UNITY_EDITOR
                    return XboxControllerState.ButtonPressed(InputHelper.ReturnXboxButton(KeyValue), (PlayerIndex)xbox);
#endif
                default:
                    return false;
            }
        }

        public bool IsReleased(int xbox = 0)
        {
            switch (Type)
            {
                case ControlType.PC:
                    return UnityEngine.Input.GetKeyUp(InputHelper.ReturnKeyCode(KeyValue));
                case ControlType.Xbox:
#if UNITY_STANDALONE || UNITY_EDITOR
                    return XboxControllerState.ButtonReleased(InputHelper.ReturnXboxButton(KeyValue), (PlayerIndex)xbox);
#endif
                default:
                    return false;
            }
        }

        #endregion

        #region Creates

        public static InputActionKey XboxButton(XboxButton btn)
        {
            return XboxButton(btn.ToString());
        }

        public static InputActionKey XboxButton(string btn)
        {
            return new InputActionKey(ControlType.Xbox, btn);
        }

        public static InputActionKey PCKey(KeyCode kc)
        {
            return PCKey(kc.ToString());
        }

        public static InputActionKey PCKey(string kc)
        {
            return new InputActionKey(ControlType.PC, kc);
        }

        #endregion

        public override string ToString()
        {
            return Type.ToString() + " " + KeyValue;
        }
    }
}
