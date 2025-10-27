using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public enum States
    {
        Closed,
        Closing,
        Open,
        Opening
    }

    public Transform childToMove;
    public States pauseState = States.Closed;

    [Space(10)]
    public float speed = 10f;

    [Space(10)]
    public Vector3 menuShowPlacement;
    public Vector3 menuHidePlacement;

    private Player player;

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
        if (player != null)
        {
            player.Controls.UI.Close.performed += _ => Close();
        }
    }

    public void Open()
    {
        childToMove.localPosition = menuHidePlacement;
        Time.timeScale = 0;
        pauseState = States.Opening;
    }

    public void Close()
    {
        Time.timeScale = 1;
        pauseState = States.Closing;
        player.ToogleControlls();
    }

    public void RestartLevel()
    {
        SceneLoader.LoadPreviousScene();
    }

    public void BackToMainMenu()
    {
        SceneLoader.LoadScene(ScreneNames.MainMenu.ToString());
    }

    public void ExiGame()
    {
        StartMenu.Quit();
    }

    private void Update()
    {
        switch (pauseState)
        {
            case States.Closing:
                Move(menuHidePlacement, States.Closed);
                break;
            case States.Opening:
                Move(menuShowPlacement, States.Open);
                break;
            default:
                break;
        }
    }

    private void Move(Vector3 towards, States onSuccess)
    {
        childToMove.localPosition = Vector3.MoveTowards(childToMove.localPosition, towards, speed);
        if (Vector3.Distance(childToMove.localPosition, towards) <= 0.01f)
        {
            childToMove.localPosition = towards;
            pauseState = onSuccess;
        }
    }
}
