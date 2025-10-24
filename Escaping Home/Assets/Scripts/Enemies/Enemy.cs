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
}
