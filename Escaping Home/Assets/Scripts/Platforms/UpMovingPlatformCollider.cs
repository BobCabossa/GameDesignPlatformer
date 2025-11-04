using UnityEngine;

public class UpMovingPlatformCollider : MonoBehaviour
{
    public UpMovingPlatform Platform;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (Platform != null)
            Platform.StartMoving(collision.gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (Platform != null)
            Platform.StartMoving(null);
    }
}
