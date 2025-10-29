using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float moveSpeed = 1;

    public Transform WallChecker;
    public SpriteRenderer SpriteRenderer;
    public Collider2D Collider;

    /// <summary> This get called at the end of <see cref="TurnAround"/> </summary>
    protected abstract void ChildTurnAround();

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
        {
            collision.gameObject.GetComponent<Player>().Die();
        }
    }

    public void TurnAround()
    {
        SpriteRenderer.flipX = !SpriteRenderer.flipX;
        TurnTransform(WallChecker);
        ChildTurnAround();
    }

    protected void TurnTransform(Transform transform)
    {
        Vector2 turnPoint = new(transform.localPosition.x * -1, transform.localPosition.y);
        transform.localPosition = turnPoint;
    }
}
