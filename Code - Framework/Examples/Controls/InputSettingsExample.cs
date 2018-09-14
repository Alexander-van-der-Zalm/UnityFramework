using UnityEngine;
using System.Collections;
using Framework.Input;

[System.Serializable][CreateAssetMenu(menuName = "Examples/InputExample")]
public class InputSettingsExample : ScriptableObject
{
    public int PlayerIndex;
    public InputAxis Horizontal, Vertical;
    public InputAction Jump;

    public void UpdatePlayerIndex()
    {
        // Set XboxPlayerIndex
        Jump.PlayerIndex = this.PlayerIndex;
        Horizontal.PlayerIndex = this.PlayerIndex;
        Vertical.PlayerIndex = this.PlayerIndex;
    }

    // Every time you add or reset the component
    public void Reset()
    {
        // Set some nice default values
        Jump = InputAction.Create(KeyCode.Space, XboxButton.A, PlayerIndex);
        Horizontal = InputAxis.Default(DirectionInput.Horizontal, PlayerIndex);
        Vertical = InputAxis.Default(DirectionInput.Vertical, PlayerIndex);
    }
}
