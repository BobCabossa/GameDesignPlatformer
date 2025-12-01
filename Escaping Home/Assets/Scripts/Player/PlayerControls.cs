using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public Player Player;
    private InputSystemActions Controls;

    public void OverrideControls(string rebinds) => Controls.LoadBindingOverridesFromJson(rebinds);
    public void RemoveOverride() => Controls.RemoveAllBindingOverrides();
    public void DisableControls() => Controls.Disable();
    private void OnDestroy() => Controls?.Dispose();

    public void Awake()
    {
        CreateControls();
        AssignControls();
        Controls.Disable();
        _ = WaitForControlsToGoBack();
    }

    private void CreateControls()
    {
        Controls = new();
        Controls.Player.Enable();
        Controls.UI.Disable();

        if (PlayerPrefs.HasKey("rebinds"))
        {
            string json = PlayerPrefs.GetString("rebinds");
            Controls.asset.LoadBindingOverridesFromJson(json);
        }
    }

    private void AssignControls()
    {
        Controls.Player.Pause.performed += Player.OnPause;

        Controls.Player.Move.performed += Player.Movement.OnMove;
        Controls.Player.Move.canceled += Player.Movement.OnMoveStop;

        Controls.Player.Jump.performed += Player.Jump.JumpRequested;

        if (Player.FindPauseMenu(out PauseMenu menu))
            Controls.UI.Close.performed += menu.OnPLayerClose;
    }

    private async Awaitable WaitForControlsToGoBack()
    {
        await Awaitable.WaitForSecondsAsync(Player.SecBeforePlayerGetControl);
        Controls.Enable();
    }

    public void ToogleControlls()
    {
        if (Controls.Player.enabled)
        {
            Controls.Player.Disable();
            Controls.UI.Enable();
        }
        else
        {
            Controls.Player.Enable();
            Controls.UI.Disable();
        }
    }
}
