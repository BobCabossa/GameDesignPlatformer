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
        Controls = new();
        Controls.Player.Pause.performed += OnPause;
        PlayerMovement.SetupControllers();
    }

    private void OnEnable()
    {
        Controls.Player.Enable();
    }

    private void OnDisable()
    {
        Controls.Player.Disable();
    }

    private void OnDestroy()
    {
        Controls.Dispose();
    }

    private void OnPause(InputAction.CallbackContext movement)
    {
        PauseMenu menu = FindAnyObjectByType<PauseMenu>();

        if (menu == null)
        {
            Debug.Log("No pause menu in sceen");
            return;
        }

        ToogleControlls();
        menu.Open();
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

    public void Die()
    {
        SceneLoader.LoadPreviousScene();
    }
}
