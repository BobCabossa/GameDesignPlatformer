using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Scripts and comp.")]
    [Space(10), SerializeField]
    private Player Player;

    [SerializeField, Space(10)]
    private Transform rayOrigin;

    [SerializeField]
    private float rayDistance = 0.1f;

    [SerializeField]
    private float rayVerticalSpacing = 0.5f;

    private float moveInput;
    private int wallLayerMask;

    private void Start()
    {
        wallLayerMask = LayerMask.GetMask("Ground");
    }

    public void SetupControllers()
    {
        Player.Controls.Player.Move.performed += OnMove;
        Player.Controls.Player.Move.canceled += _ => OnStop();

        Player.Controls.Player.Jump.performed += _ => Jump();
    }

    private void OnMove(InputAction.CallbackContext movement)
    {
        moveInput = movement.ReadValue<Vector2>().x;
    }

    private void OnStop()
    {
        moveInput = 0;
    }

    private void Jump()
    {
        if (Player.jumpState != Player.JumpState.Jumping)
        {
            Player.jumpState = Player.JumpState.Jumping;
            Player.Rigidbody.linearVelocityY = 0;
            Player.Rigidbody.AddForceY(Player.JumpForce);
        }
    }

    private void FixedUpdate()
    {
        float move = moveInput * Player.MoveSpeed;
        Player.Rigidbody.linearVelocityX = AllowPlayerToMove(move) ? move : 0;
    }

    private bool AllowPlayerToMove(float move)
    {
        Vector2 direction = new(Mathf.Sign(move), 0);
        Vector2[] rayStartOffsets = new Vector2[]
        {
            new(0,  rayVerticalSpacing),// Top
            Vector2.zero,               // Middle
            new(0, -rayVerticalSpacing) // Bottom
        };

        foreach (var offset in rayStartOffsets)
        {
            Vector2 origin = (Vector2)rayOrigin.position + offset;
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayDistance, wallLayerMask);

            if (hit.collider != null)
            {
                Debug.DrawRay(origin, direction * hit.distance, Color.red);
                return false;
            }
            else
            {
                Debug.DrawRay(origin, direction * rayDistance, Color.green);
            }
        }

        return true;
    }
}
