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
            Platform.Player = ColliderHelper.GetType<Player>(collision);
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
            ColliderHelper.GetType<Player>(collision).PlayerMovement.SetPlatformVelocity(Vector2.zero);
        }
    }
}
