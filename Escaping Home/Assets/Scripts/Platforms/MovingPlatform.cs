using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 1f;

    [SerializeField] 
    private float pauseTime = 1f;
    private float pauseTimer;

    [Header("Reference")]
    [SerializeField]
    private Transform platform;

    public new Rigidbody2D rigidbody;

    [SerializeField]
    private Transform startpoint;

    [SerializeField]
    private Transform endpoint;

    private Transform target;

    [Header("Gizmos")]
    [SerializeField]
    private Color GizmorColor = Color.cyan;

    [SerializeField]
    private float GizmorRadius = 0.5f;

    public Player Player;

    private void Awake()
    {
        startpoint.position = new(startpoint.position.x, transform.position.y);
        endpoint.position = new(endpoint.position.x, transform.position.y);
    }

    private void Start()
    {
        platform.position = startpoint.position;
        target = endpoint;
    }

    // Using late update to update after the player
    private void LateUpdate()
    {
        if (pauseTimer > 0)
        {
            pauseTimer -= Time.fixedDeltaTime;
            return;
        }

        Vector2 currentPosition = rigidbody.position;
        Vector2 targetPosition = target.position;

        if (Player != null)
        {
            float move = target == startpoint ? -speed : speed;
            Player.PlayerMovement.SetPlatformVelocity(new(move, 0));
        }

        Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.fixedDeltaTime);
        rigidbody.MovePosition(newPosition);

        if (Vector2.Distance(newPosition, targetPosition) < 0.01f)
        {
            target = target == startpoint ? endpoint : startpoint;
            pauseTimer = pauseTime;
        }
    }

    // Debugging and setup
    private void OnDrawGizmos()
    {
        if (startpoint != null && endpoint != null)
        {
            Gizmos.color = GizmorColor;
            Gizmos.DrawLine(startpoint.position, endpoint.position);
            Gizmos.DrawSphere(startpoint.position, GizmorRadius);
            Gizmos.DrawSphere(endpoint.position, GizmorRadius);
        }
    }
}
