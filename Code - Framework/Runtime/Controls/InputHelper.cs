using UnityEngine;
using System.Collections;
using System;

namespace Framework.Input
{
    public class InputHelper
    {
        public static XboxButton ReturnXboxButton(string str)
        {
            return ParseEnum<XboxButton>(str);
        }

        public static XboxAxis ReturnXboxAxis(string str)
        {
            return ParseEnum<XboxAxis>(str);
        }

        public static ControlType AxisKeyToControl(InputAxisKey.AxisKeyType key)
        {
            switch (key)
            {
                default:
                case InputAxisKey.AxisKeyType.PC:
                    return ControlType.PC;
                case InputAxisKey.AxisKeyType.Axis:
                    return ControlType.Xbox;
                case InputAxisKey.AxisKeyType.Dpad:
                    return ControlType.Xbox;
            }
        }

        //public static XboxDPad ReturnXboxDPad(string str)
        //{
        //    return ParseEnum<XboxDPad>(str);
        //}

        public static KeyCode ReturnKeyCode(string str)
        {
            return ParseEnum<KeyCode>(str);
        }

        public static T ParseEnum<T>(string value) where T : struct, IConvertible, IComparable, IFormattable
        {
            T en = (T)Enum.Parse(typeof(T), value, true);
            if (!Enum.IsDefined(typeof(T), en))
                Debug.LogError("String " + value + " is not contained in enumtype:" + typeof(T).ToString());
            return en;
        }

        [SerializeField]
        private static string[] keyCodeOptions;
        public static string[] KeyCodeOptions { get { return keyCodeOptions != null ? keyCodeOptions : keyCodeOptions = Enum.GetNames(typeof(KeyCode)); } }

        //[SerializeField]
        //private static string[] dpadOptions;
        //public static string[] DPadOptions { get { return dpadOptions != null ? dpadOptions : dpadOptions = Enum.GetNames(typeof(XboxDPad)); } }//new string[] { "left", "right", "up", "down" }; } }

        [SerializeField]
        private static string[] xboxAxixOptions;
        public static string[] XboxAxixOptions { get { return xboxAxixOptions != null ? xboxAxixOptions : xboxAxixOptions = Enum.GetNames(typeof(XboxAxis)); } }

        [SerializeField]
        private static string[] xboxButtonOptions;
        public static string[] XboxButtonOptions { get { return xboxButtonOptions != null ? xboxButtonOptions : xboxButtonOptions = Enum.GetNames(typeof(XboxButton)); } }
    }
}
