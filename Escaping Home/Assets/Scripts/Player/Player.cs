using UnityEngine;

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

    public void Die()
    {
        Debug.Log("Player Died");
        SceneLoader.LoadPreviousScene();
    }
}
