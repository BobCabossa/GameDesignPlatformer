using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Scripts and comp.")]
    [Space(10), SerializeField]
    private Player Player;

    private float moveInput;

    public void SetupControllers()
    {
        Player.Controls.Player.Move.performed += OnMove;
        Player.Controls.Player.Move.canceled += _ => OnStop();

        Player.Controls.Player.Jump.performed += _ => Jump();
    }

    private void OnMove(InputAction.CallbackContext movement)
    {
        moveInput = movement.ReadValue<Vector2>().x;
    }

    private void OnStop()
    {
        moveInput = 0;
    }

    private void Jump()
    {
        if (Player.jumpState != Player.JumpState.Jumping)
        {
            Player.jumpState = Player.JumpState.Jumping;
            Player.Rigidbody.linearVelocityY = 0;
            Player.Rigidbody.AddForceY(Player.JumpForce);
        }
    }

    private void FixedUpdate()
    {
        Player.Rigidbody.linearVelocityX = moveInput * Player.MoveSpeed;
    }
}
