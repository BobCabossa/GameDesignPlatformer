using UnityEngine;

public class SquashPlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HitPlayer(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HitPlayer(collision.collider);
    }

    private void HitPlayer(Collider2D collision)
    {
        if (ColliderHelper.IsIt<Player>(collision))
        {
            collision.transform.root.GetComponent<Player>().Die();
        }
    }
}
