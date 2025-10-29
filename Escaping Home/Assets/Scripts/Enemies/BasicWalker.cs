using UnityEngine;

public class BasicWalker : Enemy
{
    public Rigidbody2D _rigidbody;
    private bool walkingRight = false;

    protected override void ChildTurnAround()
    {
        walkingRight = !walkingRight;
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocityX = walkingRight ? moveSpeed : -moveSpeed;
    }
}
