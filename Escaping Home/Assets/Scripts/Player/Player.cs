using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public enum JumpState
    {
        Grounded,
        Jumping,
        CoyoteTime
    }

    [Header("Plaer scripts")]
    public PlayerControls Controls;
    public PlayerMovement Movement;
    public PlayerJump Jump;
    public PlayerNormalCollider NormalCollider;
    public PlayerTriggerCollider TriggerCollider;

    [Header("Other scripts")]
    public ThoughtBubble ThoughtBubble;

    [Space(10)]
    public float SecBeforePlayerGetControl = 0.1f;

    [Header("Movement")]
    public float MoveSpeed = 5f;

    [Space(5)]
    public JumpState jumpState = JumpState.Grounded;
    public float JumpForce = 40;
    public float JumpCoyoteTime = 0.2f;

    [Header("Raycast to movement")]
    public Transform rayOrigin;
    public float rayDistance = 0.1f;
    public List<Vector2> RayStartPoints = new()
    {
        new(0,  0.5f),  // Top
        Vector2.zero,   // Middle
        new(0, -0.5f)   // Bottom
    };

    [Header("Other")]
    public Rigidbody2D Rigidbody;

    private void Awake()
    {
        if (SceneLoader.FirstLoad())
            SceneLoader.SetSceneToActiveScene();
    }

    private void FixedUpdate()
    {
        Movement.Move();
        Jump.CoyoteTime();
        Jump.Jump();
    }

    public bool FindPauseMenu(out PauseMenu menu)
    {
        menu = FindAnyObjectByType<PauseMenu>();

        if (menu == null)
        {
            Debug.Log("No game menu in scene!");
            return false;
        }
        return true;
    }

    public void OnPause(InputAction.CallbackContext movement)
    {
        if (FindPauseMenu(out PauseMenu menu))
            menu.Open();
    }

    public void Win()
    {
        if (FindPauseMenu(out PauseMenu menu))
            menu.PlayerWon();

        Level.IfBeatHighestSave();
    }

    public void Die()
    {
        SceneLoader.LoadPreviousScene();
    }
}
