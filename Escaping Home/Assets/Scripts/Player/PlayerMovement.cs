using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 40;

    [Space(10), SerializeField]
    private Player Player;

    [SerializeField]
    private Rigidbody2D _rigidbody;

    private Vector2 moveInput;

    public void SetupControllers()
    {
        Player.Controls.Player.Move.performed += OnMove;
        Player.Controls.Player.Move.canceled += _ => OnStop();

        Player.Controls.Player.Jump.performed += _ => Jump();
    }

    private void OnMove(InputAction.CallbackContext movement)
    {
        moveInput = movement.ReadValue<Vector2>();
    }

    private void OnStop()
    {
        moveInput = Vector2.zero;
    }

    private void Jump()
    {
        _rigidbody.AddForceY(jumpForce);
    }

    private void FixedUpdate()
    {
        var velocity = moveInput * moveSpeed;
        velocity.y = _rigidbody.linearVelocity.y;
        _rigidbody.linearVelocity = velocity;
    }
}
