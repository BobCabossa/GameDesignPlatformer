using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] 
    private LayerMask groundLayers;
    
    [SerializeField] 
    private float rayDistance = 1.0f;

    [Header("Scripts and comp.")]
    [Space(10), SerializeField]
    private Player Player;

    [SerializeField]
    private Rigidbody2D _rigidbody;

    private float moveInput;

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
        if (Player.IsGrounded)
        {
            Player.IsGrounded = false;
            _rigidbody.AddForceY(Player.JumpForce);
        }
    }

    public void LandOnGround()
    {
        if (Player.IsGrounded)
            return;

        Vector2 origin = transform.position;
        Vector2 direction = Vector2.down;
        
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayDistance, groundLayers);
        Debug.DrawRay(origin, direction * rayDistance, Color.red);

        if (hit.collider != null)
        {
            Player.IsGrounded = true;
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocityX = moveInput * Player.MoveSpeed;
    }
}
