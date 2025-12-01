using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Movement spped")]
    public float moveSpeed = 1;
    public float moveSpeedDiviation = 25f;

    public Transform WallChecker;
    public SpriteRenderer SpriteRenderer;
    public Collider2D Collider;

    /// <summary> This get called at the end of <see cref="TurnAround"/> </summary>
    protected abstract void ChildTurnAround();
    
    protected virtual void Awake()
    {
        float diviation = Random.Range(-moveSpeedDiviation, moveSpeedDiviation);
        moveSpeed += moveSpeed * (diviation / 100);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathZone"))
        {
            Destroy(gameObject);
        }
        else if (ColliderHelper.IsIt<Player>(collision))
        {
            ColliderHelper.GetType<Player>(collision).Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ColliderHelper.IsIt<Player>(collision))
        {
            ColliderHelper.GetType<Player>(collision).Die();
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
