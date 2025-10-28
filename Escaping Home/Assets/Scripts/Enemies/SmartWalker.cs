using UnityEngine;

public class SmartWalker : Enemy
{
    public Rigidbody2D _rigidbody;
    public Transform smartCollider;
    private bool walkingRight = false;

    public void TurnAround()
    {
        walkingRight = !walkingRight;
        Vector2 turnPoint = new(smartCollider.localPosition.x * -1, smartCollider.localPosition.y);
        smartCollider.localPosition = turnPoint;
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocityX = walkingRight ? moveSpeed : -moveSpeed;
    }
}
