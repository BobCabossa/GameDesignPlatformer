using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathZone"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<Player>().Die();
    }
}
