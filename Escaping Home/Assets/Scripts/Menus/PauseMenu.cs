using System.Net;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public enum States
    {
        Closed,
        Closing,
        Open,
        Opening
    }

    public TextMeshProUGUI LevelText;
    public GameObject background;
    public GameObject WinScreen;

    public Transform childToMove;
    public States pauseState = States.Closed;

    [Space(10)]
    public float speed = 10f;

    [Space(10)]
    public Vector3 menuShowPlacement;
    public Vector3 menuHidePlacement;

    private Player player;

    public void RestartLevel() => SceneLoader.LoadPreviousScene();
    public void BackToMainMenu() => SceneLoader.LoadScene(ScreneNames.MainMenu);
    public void ExiGame() => MainMenuController.Quit();

    private void Awake()
    {
        WinScreen.SetActive(false);
        background.SetActive(false);
        Scene scene = SceneManager.GetActiveScene();
        LevelText.text = AddSpacesToSentence(scene.name);
    }

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
        if (player != null)
        {
            player.Controls.UI.Close.performed += _ => Close();
        }
    }

    public static string AddSpacesToSentence(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        return Regex.Replace(text, "(?<!^)([A-Z])", " $1");
    }

    public void PlayerWon()
    {
        background.SetActive(true);
        WinScreen.SetActive(true);
    }

    public void Open()
    {
        Time.timeScale = 0;
        background.SetActive(true);
        pauseState = States.Opening;
    }

    public void Close()
    {
        Time.timeScale = 1;
        background.SetActive(false);
        pauseState = States.Closing;
        player.ToogleControlls();
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
