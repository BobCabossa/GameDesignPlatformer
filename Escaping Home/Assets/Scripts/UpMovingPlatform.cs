using UnityEngine;

public class UpMovingPlatform : MonoBehaviour
{
    public enum States
    {
        Idle,
        Up,
        Down
    }

    public float speed;

    [Header("Don't touch")]
    public Transform platform;
    public Transform startPoint;
    public Transform endPoint;

    public States state;

    public void StartMoving()
    {
        state = States.Up;
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            default: break;
            case States.Up:
                Move(endPoint.position, States.Down);
                break;
            case States.Down:
                Move(startPoint.position, States.Idle);
                break;
        }
    }

    private void Move(Vector3 targetPoint, States onSuccess)
    {
        float step = speed * Time.deltaTime;
        platform.position = Vector3.MoveTowards(platform.position, targetPoint, step);
        if (Vector3.Distance(platform.position, targetPoint) < 0.001f)
        {
            state = onSuccess;
        }
    }
}
