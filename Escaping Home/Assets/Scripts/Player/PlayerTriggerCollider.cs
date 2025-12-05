using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerTriggerCollider : MonoBehaviour
{
    public Player Player;
    
    private void Awake()
    {
        Collider2D collider = GetComponent<Collider2D>();

        if (!collider.isTrigger)
            Debug.LogWarning("Player trigger collider, is not a trigger... setting it to trigger");

        collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Player.jumpState = Player.JumpState.Grounded;
            Player.Jump.ResetCoyoteTimer();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
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
