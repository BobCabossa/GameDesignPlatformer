using UnityEngine;

public class BasicWalker : Enemy
{
    public Rigidbody2D _rigidbody;

    private void FixedUpdate()
    {
        _rigidbody.linearVelocityX = -moveSpeed;
    }
}
