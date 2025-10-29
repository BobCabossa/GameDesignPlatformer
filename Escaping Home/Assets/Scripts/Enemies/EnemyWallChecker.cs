using UnityEngine;

public class EnemyWallChecker : MonoBehaviour
{
    public Enemy Enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            Enemy.TurnAround();
        }
    }
}
