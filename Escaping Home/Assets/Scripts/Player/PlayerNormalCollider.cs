using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerNormalCollider : MonoBehaviour
{
    public Player Player;

    private void Awake()
    {
        Collider2D collider = GetComponent<Collider2D>();

        if (collider.isTrigger)
            Debug.LogWarning("Player collider, is a trigger... making it a normal collider");

        collider.isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathZone"))
        {
            Player.Die();
        }
        else if (collision.CompareTag("WinZone"))
        {
            Destroy(Player.gameObject);
            Player.Win();
        }
    }
}
