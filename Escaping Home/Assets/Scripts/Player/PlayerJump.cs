using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    public Player Player;

    private float JumpCoyoteTime = 0.2f;
    private bool jumpRequested = false;

    public void ResetCoyoteTimer()
    {
        JumpCoyoteTime = Player.JumpCoyoteTime;
    }

    public void JumpRequested(InputAction.CallbackContext _)
    {
        jumpRequested = true;
    }

    private void FixedUpdate()
    {
        CoyoteTime();   
        Jump();
    }

    private void CoyoteTime()
    {
        if (Player.jumpState == Player.JumpState.CoyoteTime)
        {
            JumpCoyoteTime -= Time.deltaTime;
            if (JumpCoyoteTime <= 0)
            {
                JumpCoyoteTime = Player.JumpCoyoteTime;
                Player.jumpState = Player.JumpState.Jumping;
            }
        }
    }

    private void Jump()
    {
        if (!jumpRequested)
            return;

        jumpRequested = false;
        if (Player.jumpState != Player.JumpState.Jumping && Player.Rigidbody != null)
        {
            Player.jumpState = Player.JumpState.Jumping;

            // Calculate jump velocity
            float jumpVelocity = Mathf.Sqrt(2 * Player.JumpForce * Mathf.Abs(Physics2D.gravity.y));
            Player.Rigidbody.linearVelocityY = jumpVelocity;
        }
    }
}
