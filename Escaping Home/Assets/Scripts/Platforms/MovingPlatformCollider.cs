using UnityEngine;

public class MovingPlatformCollider : MonoBehaviour
{
    public MovingPlatform Platform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            Platform.TurnAround();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.isTrigger) return;

        var playerRoot = collision.collider.transform.root;
        if (playerRoot.CompareTag("Player"))
        {
            Platform.Player = playerRoot.GetComponent<Player>();
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Platform.TurnAround();
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
