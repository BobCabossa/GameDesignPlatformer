using UnityEngine;

public class MenuMover : MonoBehaviour
{
    public enum States
    {
        Closed,
        Closing,
        Open,
        Opening
    }

    [Tooltip("If nothing is asigned, it moves itself.")]
    public Transform childToMove;

    [Space(5)]
    public float speed = 10f;

    public Vector3 StartPlacement;
    public Vector3 EndPlacement;

    private States state = States.Closed;

    public void Open() => state = States.Opening;
    public void Close() => state = States.Closing;

    private void Awake()
    {
        if (childToMove == null)
        {
            childToMove = transform;
        }

        childToMove.localPosition = StartPlacement;
    }

    private void Update()
    {
        switch (state)
        {
            case States.Closing:
                MoveTo(StartPlacement, States.Closed);
                break;
            case States.Opening:
                MoveTo(EndPlacement, States.Open);
                break;
            default:
                break;
        }
    }

    private void MoveTo(Vector3 towards, States onSuccess)
    {
        childToMove.localPosition = Vector3.MoveTowards(childToMove.localPosition, towards, speed);
        if (Vector3.Distance(childToMove.localPosition, towards) <= 0.01f)
        {
            childToMove.localPosition = towards;
            state = onSuccess;
        }
    }
}
