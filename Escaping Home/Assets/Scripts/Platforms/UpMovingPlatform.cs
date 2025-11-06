using UnityEngine;

public class UpMovingPlatform : MonoBehaviour
{
    public enum States { Idle, Up, Down }

    public float speed = 5;
    public float delayBeforeMoving = 1;

    [Header("Don't touch!")]
    public Rigidbody2D platformRigidbody;
    public Transform startPoint;
    public Transform endPoint;

    [Header("Debug values.")]
    [SerializeField]
    private float delay = 0;

    [SerializeField]
    private bool playerOnPlatform = false;
    
    [SerializeField]
    private States state = States.Idle;

    [SerializeField]
    private States oldState = States.Idle;

    private void SetState(States newState)
    {
        oldState = state;
        state = newState;
    }

    public void StartMoving(GameObject player)
    {
        playerOnPlatform = player != null;
        if (state == States.Idle)
        {
            delay = delayBeforeMoving;
            SetState(States.Up);
        }
    }

    // To wait for the player to move off the platform, then move down
    private void Update()
    {
        //if (delay == delayBeforeMoving) return;
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


    private void FixedUpdate()
    {
        switch (state)
        {
            case States.Up:
                Move(endPoint.position);
                break;
            case States.Down:
                Move(startPoint.position);
                break;
            default: break;
        }
    }

    private void Move(Vector2 targetPoint)
    {
        delay -= Time.deltaTime;
        if (delay > 0) return;

        float step = speed * Time.deltaTime;
        platformRigidbody.MovePosition(Vector2.MoveTowards(platformRigidbody.position, targetPoint, step));
        if (Vector2.Distance(platformRigidbody.position, targetPoint) < 0.001f)
        {
            Player player = FindAnyObjectByType<Player>();
            if (player != null)
                player.Rigidbody.linearVelocityY = 0f;
            
            SetState(States.Idle);
            delay = delayBeforeMoving;
        }
    }

    // Debugging and setup
    private void OnDrawGizmos()
    {
        if (startPoint != null && endPoint != null)
        {
            Gizmos.color = GizmosSettings.Color;
            Gizmos.DrawLine(startPoint.position, endPoint.position);
            Gizmos.DrawSphere(startPoint.position, GizmosSettings.Radius);
            Gizmos.DrawSphere(endPoint.position, GizmosSettings.Radius);
        }
    }
}
