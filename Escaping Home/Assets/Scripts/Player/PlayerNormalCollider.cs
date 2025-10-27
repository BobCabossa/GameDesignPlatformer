using UnityEngine;

public class PlayerNormalCollider : MonoBehaviour
{
    public Player Player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathZone"))
        {
            Player.Die();
        }
        else if (collision.CompareTag("WinZone"))
        {
            Debug.Log("Player won this level!");
        }
    }
}
