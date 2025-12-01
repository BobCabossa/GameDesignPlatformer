using UnityEngine;

public class PlayerTriggerCollider : MonoBehaviour
{
    public Player Player;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Player.jumpState = Player.JumpState.Grounded;
            Player.Jump.ResetCoyoteTimer();
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
}
