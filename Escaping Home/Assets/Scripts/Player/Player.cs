using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public enum JumpState
    {
        Grounded,
        Jumping,
        CoyoteTime
    }

    [Header("Plaer scripts")]
    public PlayerMovement PlayerMovement;
    public PlayerNormalCollider PlayerNormalCollider;
    public PlayerTriggerCollider PlayerTriggerCollider;

    [Header("Movement")]
    public float SecBeforePlayerGetControl = 0.1f;

    public float MoveSpeed = 5f;

    [Space(5)]
    public JumpState jumpState = JumpState.Grounded;
    public float JumpForce = 40;
    public float JumpCoyoteTime = 0.2f;

    [Header("Other")]
    public Rigidbody2D Rigidbody;

    // This can't be seen in the inspector
    public InputSystemActions Controls;

    private void Awake()
    {
        if (SceneLoader.FirstLoad())
            SceneLoader.SetSceneToActiveScene();

        CreateControls();
        Controls.Disable();
        _ = WaitForControlsToGoBack();
    }

    private void Start()
    {
        Controls.Enable();
    }

    private void OnEnable()
    {
        Controls?.Player.Enable();
    }

    private void OnDisable()
    {
        Controls?.Player.Disable();
    }

    // When the scene is destroyed by scene change
    private void OnDestroy()
    {
        Controls?.Dispose();
    }

    private async Awaitable WaitForControlsToGoBack()
    {
        await Awaitable.WaitForSecondsAsync(SecBeforePlayerGetControl);
        Controls.Enable();
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

        Controls.Player.Pause.performed += OnPause;

        Controls.Player.Move.performed += PlayerMovement.OnMove;
        Controls.Player.Move.canceled += PlayerMovement.OnMoveStop;

        Controls.Player.Jump.performed += PlayerMovement.JumpRequested;

        if (FindPauseMenu(out PauseMenu menu))
            Controls.UI.Close.performed += menu.OnPLayerClose;
    }

    private void OnPause(InputAction.CallbackContext movement)
    {
        if (FindPauseMenu(out PauseMenu menu))
            menu.Open();
    }

    private bool FindPauseMenu(out PauseMenu menu)
    {
        menu = FindAnyObjectByType<PauseMenu>();

        if (menu == null)
        {
            Debug.Log("No game menu in scene!");
            return false;
        }
        return true;
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

    public void Win()
    {
        if (FindPauseMenu(out PauseMenu menu))
            menu.PlayerWon();

        Level.IfBeatHighestSave();
    }

    public void Die()
    {
        SceneLoader.LoadPreviousScene();
    }
}
