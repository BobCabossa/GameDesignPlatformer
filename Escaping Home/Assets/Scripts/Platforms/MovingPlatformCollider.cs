using UnityEngine;

public class MovingPlatformCollider : MonoBehaviour
{
    public MovingPlatform Platform;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.isTrigger) return;

        var playerRoot = collision.collider.transform.root;
        if (playerRoot.CompareTag("Player"))
        {
            Platform.Player = playerRoot.GetComponent<Player>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.isTrigger) return;

        var playerRoot = collision.collider.transform.root;
        if (playerRoot.CompareTag("Player"))
        {
            Platform.Player = null;
            playerRoot.GetComponent<Player>().PlayerMovement.SetPlatformVelocity(Vector2.zero);
        }
    }
}
