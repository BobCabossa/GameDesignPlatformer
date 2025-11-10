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

    private bool jumpRequested = false;
    private float moveInput;
    private int wallLayerMask;
    private Vector2 platformVelocity;

    private Vector2[] GetRayStartPoints() => new Vector2[]
    {
        new(0,  rayVerticalSpacing),// Top
        Vector2.zero,               // Middle
        new(0, -rayVerticalSpacing) // Bottom
    };

    private void Start()
    {
        wallLayerMask = LayerMask.GetMask("Ground");
    }

    // Inputs
    public void SetPlatformVelocity(Vector2 velocity)
    {
        platformVelocity = velocity;
    }

    public void OnMove(InputAction.CallbackContext movement)
    {
        moveInput = movement.ReadValue<Vector2>().x;
    }

    public void OnMoveStop(InputAction.CallbackContext _)
    {
        moveInput = 0;
    }

    public void JumpRequested(InputAction.CallbackContext _)
    {
        jumpRequested = true;
    }

    // Real movement
    private void FixedUpdate()
    {
        Move();
        Jump();
    }

    private void Jump()
    {
        if (!jumpRequested)
            return;

        jumpRequested = false;
        if (Player.jumpState != Player.JumpState.Jumping && Player.Rigidbody != null)
        {
            Player.jumpState = Player.JumpState.Jumping;
            Player.Rigidbody.linearVelocityY = 0;
            Player.Rigidbody.AddForceY(Player.JumpForce, ForceMode2D.Impulse);
        }
    }

    private void Move()
    {
        float move = moveInput * Player.MoveSpeed;
        float velocityX = (AllowPlayerToMove(move) ? move : 0) + platformVelocity.x;

        Player.Rigidbody.linearVelocityX = velocityX;
        Player.Rigidbody.linearVelocityY += platformVelocity.y;
    }

    private bool AllowPlayerToMove(float move)
    {
        Vector2 direction = new(Mathf.Sign(move), 0);
        Vector2[] rayStartOffsets = GetRayStartPoints();

        foreach (var offset in rayStartOffsets)
        {
            Vector2 origin = (Vector2)rayOrigin.position + offset;
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayDistance, wallLayerMask);

            if (hit.collider != null)
                return false;
        }

        return true;
    }

    private void OnDrawGizmosSelected()
    {
        if (rayOrigin == null) return;

        Vector2[] rayStartOffsets = GetRayStartPoints();
        float move = 1f;
        Vector2 direction = new(Mathf.Sign(move), 0);

        Gizmos.color = GizmosSettings.Color;
        foreach (var offset in rayStartOffsets)
        {
            Vector2 origin = (Vector2)rayOrigin.position + offset;
            Gizmos.DrawLine(origin, origin + direction * rayDistance);
        }
    }
}
