using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Player Player;

    private float moveInput;
    private int wallLayerMask;
    private Vector2 platformVelocity;

    private void Start() => wallLayerMask = LayerMask.GetMask("Ground");
    public void SetPlatformVelocity(Vector2 velocity) => platformVelocity = velocity;
    public void OnMove(InputAction.CallbackContext movement) => moveInput = movement.ReadValue<Vector2>().x;
    public void OnMoveStop(InputAction.CallbackContext _) => moveInput = 0;

    public void Move()
    {
        float move = moveInput * Player.MoveSpeed;
        float velocityX = (AllowPlayerToMove(move) ? move : 0) + platformVelocity.x;

        Player.Rigidbody.linearVelocityX = velocityX;
        Player.Rigidbody.linearVelocityY += platformVelocity.y;
    }

    private bool AllowPlayerToMove(float move)
    {
        Vector2 direction = new(Mathf.Sign(move), 0);

        foreach (var offset in Player.RayStartPoints)
        {
            Vector2 origin = (Vector2)Player.rayOrigin.position + offset;
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, Player.rayDistance, wallLayerMask);

            if (hit.collider != null)
                return false;
        }

        return true;
    }

    private void OnDrawGizmosSelected()
    {
        if (Player.rayOrigin == null) return;

        float move = 1f;
        Vector2 direction = new(Mathf.Sign(move), 0);

        Gizmos.color = GizmosSettings.Color;
        foreach (var offset in Player.RayStartPoints)
        {
            Vector2 origin = (Vector2)Player.rayOrigin.position + offset;
            Gizmos.DrawLine(origin, origin + direction * Player.rayDistance);
        }
    }
}
