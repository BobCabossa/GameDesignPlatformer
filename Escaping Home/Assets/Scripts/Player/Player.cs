using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public PlayerMovement PlayerMovement;
    public PlayerNormalCollider PlayerNormalCollider;
    public PlayerTriggerCollider PlayerTriggerCollider;

    [Header("Movement")]
    public float MoveSpeed = 5f;
    public float JumpForce = 40;
    public float JumpCoyoteTime = 0.2f;

    [Header("Other")]
    public bool IsGrounded = true;
    public bool Jumped = false;

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
        PlayerNormalCollider.enabled = true;
        PlayerTriggerCollider.enabled = true;
        PlayerMovement.enabled = true;
        Controls.Player.Enable();
    }

    private void OnDisable()
    {
        PlayerNormalCollider.enabled = false;
        PlayerTriggerCollider.enabled = false;
        PlayerMovement.enabled = false;
        Controls.Player.Disable();
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
        Debug.Log("Player Died");
        SceneLoader.LoadPreviousScene();
    }
}
