using UnityEngine;
using Framework.Input;

public class InputHandlerExample : MonoBehaviour
{
    public DebugInfo Info;
    // Define a Player index for a controller
    public int PlayerIndex;
    // Axis can be used for both directional
    public InputAxis Horizontal, Vertical;
    // An action 
    public InputAction Jump;
    // This is an analogue axis implementation
    public InputAxis Accelerate;

    // Every time you add or reset the component
    private void Reset()
    {
        Info = new DebugInfo();
        // Set some nice default values
        Jump = InputAction.Create(KeyCode.Space, XboxButton.A, PlayerIndex);
        Horizontal = InputAxis.Default(DirectionInput.Horizontal, PlayerIndex);
        Vertical = InputAxis.Default(DirectionInput.Vertical, PlayerIndex);

        // Custom add keys (like right trigger) 
        // Note can also be done in inspector ofcourse. But this is how you would do it in code.
        Accelerate = new InputAxis(PlayerIndex);
        Accelerate.AxisKeys.Add(InputAxisKey.XboxAxis(XboxAxis.RightTrigger));
    }

    // Use this for initialization
    void Start()
    {
        // Set XboxPlayerIndex
        Jump.PlayerIndex = this.PlayerIndex;
        Horizontal.PlayerIndex = this.PlayerIndex;
        Vertical.PlayerIndex = this.PlayerIndex;
    }

    // Update is called once per frame
    void Update()
    {
        // Get all the values
        float hor = Horizontal.Value();
        float ver = Vertical.Value();
        bool JumpIsDown = Jump.IsDown();
        float accel = Accelerate.Value();

        // This is how you could feed the info to your physics controller
        Info.FakeInputFeed(hor, ver, accel, JumpIsDown);

        // This is how you do something on pressed
        if (Jump.IsPressed())
            Debug.Log("Jump Start now!");

        // Log info
        if(Info.LogInfo)
            Debug.Log("InputHandlerExample - [" + hor + "," + ver + "] [" + accel + "] Jump: " + Jump.IsDown());
    }

    [System.Serializable]
    public class DebugInfo
    {
        public bool LogInfo = false;
        public float Horizontal, Vertical, Accelerate;
        public bool JumpDown;

        public void FakeInputFeed(float hor, float ver, float accel, bool jumpDown)
        {
            Horizontal = hor;
            Vertical = ver;
            Accelerate = accel;
            JumpDown = jumpDown;
        }
    }
}

