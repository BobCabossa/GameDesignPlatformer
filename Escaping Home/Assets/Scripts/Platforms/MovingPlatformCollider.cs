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

        if (ColliderHelper.IsIt<Player>(collision))
        {
            var playerRoot = collision.collider.transform.root;
            Platform.Player = playerRoot.GetComponent<Player>();
        }
        else if (ColliderHelper.IsIt<Enemy>(collision))
        {
            Platform.TurnAround();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.isTrigger) return;

        if (ColliderHelper.IsIt<Player>(collision))
        {
            Platform.Player = null;
            var playerRoot = collision.collider.transform.root;
            playerRoot.GetComponent<Player>().PlayerMovement.SetPlatformVelocity(Vector2.zero);
        }
    }
}
