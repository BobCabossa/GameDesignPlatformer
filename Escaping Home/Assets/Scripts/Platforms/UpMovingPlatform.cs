using UnityEngine;

public class UpMovingPlatform : MonoBehaviour
{
    public enum States { Idle, Up, Down }

    public float speed = 5;
    public float delayBeforeMoving = 1;

    [Header("Don't touch!")]
    public Rigidbody2D platformRigidbody;
    public Transform TopPoint;
    public Transform BottomPoint;

    [Header("Debug values.")]
    [SerializeField]
    private float delay = 0;

    [SerializeField]
    private bool playerOnPlatform = false;

    private States state = States.Idle;
    private States oldState = States.Idle;

    private Player player;

    private void SetState(States newState)
    {
        oldState = state;
        state = newState;
    }

    public void StartMoving(Collision2D playerCollider)
    {
        playerOnPlatform = playerCollider != null;
        player = playerOnPlatform ? ColliderHelper.GetType<Player>(playerCollider) : null;

        if (state == States.Idle)
        {
            delay = delayBeforeMoving;
            SetState(States.Up);
        }
    }

    private void FixedUpdate()
    {
        HandleIdleAtTop();

        delay -= Time.deltaTime;
        if (delay > 0) return;

        switch (state)
        {
            case States.Up:
                Move(TopPoint.position, movePlayer: true);
                break;
            case States.Down:
                Move(BottomPoint.position, movePlayer: false);
                break;
            default: break;
        }
    }

    private void Move(Vector2 targetPoint, bool movePlayer)
    {
        float step = speed * Time.deltaTime;
        platformRigidbody.MovePosition(Vector2.MoveTowards(platformRigidbody.position, targetPoint, step));
        if (Vector2.Distance(platformRigidbody.position, targetPoint) < 0.001f)
        {
            if (player != null)
                player.Rigidbody.linearVelocityY = 0f;

            SetState(States.Idle);
            delay = delayBeforeMoving;
        }
    }

    private void HandleIdleAtTop()
    {
        if (playerOnPlatform)
        {
            if (state == States.Idle && oldState == States.Down)
            {
                SetState(States.Up);
            }
            return;
        }

        if (state != States.Idle) return;

        if (oldState == States.Up)
        {
            state = States.Down;
            oldState = States.Idle;
        }
    }

    // Debugging and setup
    private void OnDrawGizmos()
    {
        if (BottomPoint != null && TopPoint != null)
        {
            Gizmos.color = GizmosSettings.Color;
            Gizmos.DrawLine(BottomPoint.position, TopPoint.position);
            Gizmos.DrawSphere(BottomPoint.position, GizmosSettings.Radius);
            Gizmos.DrawSphere(TopPoint.position, GizmosSettings.Radius);
        }
    }
}
