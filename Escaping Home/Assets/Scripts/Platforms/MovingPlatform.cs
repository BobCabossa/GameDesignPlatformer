using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 1f;

    [SerializeField]
    private float pauseTime = 1f;
    private float pauseTimer;

    [SerializeField, Tooltip("With only set the start direction, not when it's moving.")]
    private bool startGoingRight = true;

    [Header("Reference")]
    [SerializeField]
    private Transform platform;

    public Rigidbody2D Rigidbody;

    public Transform LeftPoint;
    public Transform RightPoint;

    private Transform target;

    [HideInInspector]
    public Player Player;

    private void Awake()
    {
        LeftPoint.position = new(LeftPoint.position.x, transform.position.y);
        RightPoint.position = new(RightPoint.position.x, transform.position.y);
        target = startGoingRight ? RightPoint : LeftPoint;
    }

    public void TurnAround()
    {
        if (pauseTimer <= 0)
            target = target == LeftPoint ? RightPoint : LeftPoint;
    }

    private void FixedUpdate()
    {
        if (IsPlatformAllowToMove())
        {
            MovePlayer();
            MovePlatform();
        }
    }

    private bool IsPlatformAllowToMove()
    {
        if (pauseTimer < 0)
            return true;

        if (Player != null)
            Player.PlayerMovement.SetPlatformVelocity(Vector2.zero);

        pauseTimer -= Time.fixedDeltaTime;
        return false;
    }

    private void MovePlayer()
    {
        if (Player != null)
        {
            float move = target == LeftPoint ? -speed : speed;
            Player.PlayerMovement.SetPlatformVelocity(new(move, 0));
        }
    }

    private void MovePlatform()
    {
        Vector2 currentPosition = Rigidbody.position;
        Vector2 targetPosition = target.position;

        Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.fixedDeltaTime);
        Rigidbody.MovePosition(newPosition);

        if (Vector2.Distance(newPosition, targetPosition) < 0.01f)
        {
            TurnAround();
            pauseTimer = pauseTime;
        }
    }

    // Debugging and setup
    private void OnDrawGizmos()
    {
        if (LeftPoint != null && RightPoint != null)
        {
            Gizmos.color = GizmosSettings.Color;
            Gizmos.DrawLine(LeftPoint.position, RightPoint.position);
            Gizmos.DrawSphere(LeftPoint.position, GizmosSettings.Radius);
            Gizmos.DrawSphere(RightPoint.position, GizmosSettings.Radius);
        }
    }
}
