using UnityEngine;

public class FlyingEnemy : Enemy
{
    public Rigidbody2D _rigidbody;
    public Transform StartPoint;
    public Transform EndPoint;

    private bool flyingLeft = true;

    protected override void ChildTurnAround()
    {
        flyingLeft = !flyingLeft;
    }

    private void FixedUpdate()
    {
        Vector3 target = flyingLeft ? EndPoint.localPosition : StartPoint.localPosition;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, moveSpeed);
        if (Vector3.Distance(transform.localPosition, target) < 0.001f)
        {
            TurnAround();
        }
    }
}
