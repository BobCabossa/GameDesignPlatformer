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

    [Space(10), Tooltip("Don't manually asign, only for debugging!")]
    public Transform player;

    private Vector3 targetPoint;
    private Vector3 lastPlatformPosition;
    private bool forward = true;

    private void Start()
    {
        platform.position = startpoint.position;
        targetPoint = endpoint.position;
        lastPlatformPosition = platform.position;
    }

    private void Update()
    {
        Vector3 delta = platform.position - lastPlatformPosition;

        if (player != null)
        {
            player.position += delta;
        }
        
        lastPlatformPosition = platform.position;
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
