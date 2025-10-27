using UnityEngine;

public class PlayerTriggerCollider : MonoBehaviour
{
    public Player Player;

    public float JumpCoyoteTime = 0.2f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Player.IsGrounded = true;
            Player.Jumped = false;
            JumpCoyoteTime = Player.JumpCoyoteTime;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (Player.Jumped)
            {
                Player.IsGrounded = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!Player.IsGrounded && !Player.Jumped)
        {
            JumpCoyoteTime -= Time.deltaTime;
            if (JumpCoyoteTime <= 0)
            {
                JumpCoyoteTime = Player.JumpCoyoteTime;
                Player.IsGrounded = false;
            }
        }
    }
}
