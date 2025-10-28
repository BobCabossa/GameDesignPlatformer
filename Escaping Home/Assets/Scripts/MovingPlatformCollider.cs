using UnityEngine;

public class MovingPlatformCollider : MonoBehaviour
{
    public MovingPlatform Platform;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Platform.player = collision.gameObject.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Platform.player = null;
        }
    }
}
