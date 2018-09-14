using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Framework.Input
{
    [System.Serializable]
    public class InputAction
    {
        #region Fields

        public string Name;
        public List<InputActionKey> Keys;

        [SerializeField, EditorReadOnly]
        private ControlType m_LastInputType = ControlType.PC;
        [SerializeField]
        private int m_XboxPlayer;

        public int PlayerIndex { get { return m_XboxPlayer; } set { m_XboxPlayer = value; } }
        //[SerializeField,HideInInspector]
        //protected ControlScheme scheme;

        public ControlType LastInputType { get { return m_LastInputType; } }

        #endregion

        #region CTor

        public InputAction(int plyr = 0, string name = "defaultAction")
        {
            Keys = new List<InputActionKey>();

            //this.scheme = scheme;
            this.Name = name;
            this.m_XboxPlayer = plyr;
        }

        #endregion

        #region Down, Pressed, Released

        public bool IsDown()
        {
            foreach (InputActionKey key in Keys)
            {
                if (key.IsDown(m_XboxPlayer))
                    return TrueAndSetInputType(key);
            }
            return false;
        }
        public bool IsPressed()
        {
            foreach (InputActionKey key in Keys)
            {
                if (key.IsPressed(m_XboxPlayer))
                    return TrueAndSetInputType(key);
            }
            return false;
        }

        public bool IsReleased()
        {
            foreach (InputActionKey key in Keys)
            {
                if (key.IsReleased(m_XboxPlayer))
                    return TrueAndSetInputType(key);
            }
            return false;
        }

        private bool TrueAndSetInputType(InputActionKey key)
        {
            m_LastInputType = key.Type;
            return true;
        }

        #endregion

        #region Creates

        public void AddXboxButton(XboxButton btn)
        {
            Keys.Add(InputActionKey.XboxButton(btn));
        }

        public void AddXboxButton(string btn)
        {
            Keys.Add(InputActionKey.XboxButton(btn));
        }

        public void AddPCKey(KeyCode kc)
        {
            Keys.Add(InputActionKey.PCKey(kc));
        }

        public void AddPCKey(string kc)
        {
            Keys.Add(InputActionKey.PCKey(kc));
        }

        public static InputAction Create(string pc, XboxButton xb = XboxButton.None, int index = 0, string name = "")
        {
            InputAction ac = new InputAction(index, name);
            ac.AddPCKey(pc);
            if (xb != XboxButton.None)
                ac.AddXboxButton(xb);
            return ac;
        }

        public static InputAction Create(KeyCode pc, XboxButton xb = XboxButton.None, int index = 0, string name = "")
        {
            InputAction ac = new InputAction(index, name);
            ac.AddPCKey(pc);
            if (xb != XboxButton.None)
                ac.AddXboxButton(xb);
            return ac;
        }

        #endregion
    }
}
