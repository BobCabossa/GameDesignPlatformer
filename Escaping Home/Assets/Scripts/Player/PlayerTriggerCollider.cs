using UnityEngine;

public class PlayerTriggerCollider : MonoBehaviour
{
    public Player Player;

    [Tooltip("This is a debug value, if you want to set this: go to the player script!")]
    public float JumpCoyoteTime = 0.2f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Player.jumpState = Player.JumpState.Grounded;
            JumpCoyoteTime = Player.JumpCoyoteTime;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (Player.jumpState == Player.JumpState.Grounded)
            {
                Player.jumpState = Player.JumpState.CoyoteTime;
            }
        }
    }

    private void FixedUpdate()
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
}
