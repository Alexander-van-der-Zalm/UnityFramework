using UnityEngine;

namespace Framework.Input
{
    [System.Serializable]
    public struct ActionCopy
    {
        public bool IsPressed { get { return isPressed; } }
        public bool IsReleased { get { return isReleased; } }
        public bool IsDown { get { return isDown; } }

        [EditorReadOnly,SerializeField]
        private bool isPressed, isReleased, isDown;

        public ActionCopy(InputAction action)
        {
            isPressed = action.IsPressed();
            isReleased = action.IsReleased();
            isDown = action.IsDown();
        }
    }
}
