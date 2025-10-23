using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;

    [Header("Movement")]
    public float smoothSpeed = 5f;
    public Vector2 offset;

    [Header("Bounds")]
    public Transform minBounds;
    public Transform maxBounds;

    private float zValue = -10;

    private void Awake()
    {
        zValue = transform.position.z;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // New position
        Vector3 desiredPosition = new(
            target.position.x + offset.x,
            target.position.y + offset.y,
            transform.position.z
        );
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Keep new position inside bounds
        smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minBounds.position.x, maxBounds.position.x);
        smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minBounds.position.y, maxBounds.position.y);
        //smoothedPosition.z = zValue;

        transform.position = smoothedPosition;
    }
}
