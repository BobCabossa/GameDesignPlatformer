using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    [Header("Reference")]
    [SerializeField]
    private Transform platform;

    [SerializeField]
    private Transform startpoint;

    [SerializeField]
    private Transform endpoint;

    private Vector3 targetPoint;
    private bool forward = true;

    private void Start()
    {
        platform.position = startpoint.position;
        targetPoint = endpoint.position;
    }

    private void FixedUpdate()
    {
        float step = speed * Time.deltaTime;
        platform.position = Vector3.MoveTowards(platform.position, targetPoint, step);

        if (Vector3.Distance(platform.position, targetPoint) < 0.001f)
        {
            // Snap to point
            platform.position = targetPoint;

            forward = !forward;
            targetPoint = forward ? endpoint.position : startpoint.position;
        }
    }
}
