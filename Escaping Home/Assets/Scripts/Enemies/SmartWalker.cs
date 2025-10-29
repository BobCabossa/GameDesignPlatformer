using UnityEngine;

public class SmartWalker : Enemy
{
    public Rigidbody2D _rigidbody;
    public Transform smartCollider;
    private bool walkingRight = false;

    protected override void ChildTurnAround()
    {
        walkingRight = !walkingRight;
        TurnTransform(smartCollider);
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocityX = walkingRight ? moveSpeed : -moveSpeed;
    }
}
