using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovement PlayerMovement;
    public PlayerCollider PlayerCollider;

    public InputSystemActions Controls;

    private void Awake()
    {
        Controls = new();
        PlayerMovement.SetupControllers();
    }
    private void OnEnable()
    {
        PlayerCollider.enabled = true;
        PlayerMovement.enabled = true;
        Controls.Player.Enable();
    }

    private void OnDisable()
    {
        PlayerCollider.enabled = false;
        PlayerMovement.enabled = false;
        Controls.Player.Disable();
    }

    public void Die()
    {
        Debug.Log("Player Died");
        SceneLoader.LoadPreviousScene();
    }
}
