using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private enum GameMenuOpen
    {
        None,
        Normal,
        Settings,
        Win
    }

    public TextMeshProUGUI LevelText;

    public MenuMover pauseMenu;
    public GameObject background;
    public GameObject WinScreen;
    public MenuMover Settings;

    private Player player;
    private GameMenuOpen menuOpen = GameMenuOpen.None;

    public void RestartLevel() => SceneLoader.LoadPreviousScene();
    public void BackToMainMenu() => SceneLoader.LoadScene(SceneNames.MainMenu);
    public void ExiGame() => MainMenuController.Quit();

    private void Awake()
    {
        WinScreen.SetActive(false);
        background.SetActive(false);

        Scene scene = SceneManager.GetActiveScene();
        LevelText.text = AddSpacesToSentence(scene.name);
        player = FindAnyObjectByType<Player>();
    }

    public void OnPLayerClose(InputAction.CallbackContext _)
    {
        switch (menuOpen)
        {
            case GameMenuOpen.Normal:
                Close();
                break;
            case GameMenuOpen.Settings:
                CloseSettings();
                break;
            default: break;
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
        player.Controls.Disable();
        menuOpen = GameMenuOpen.Win;
        background.SetActive(true);
        WinScreen.SetActive(true);
    }

    public async void Open()
    {
        // Used for the first open, there are a double trigger of event (Open and close).
        await Awaitable.NextFrameAsync();

        Time.timeScale = 0;
        background.SetActive(true);
        pauseMenu.Open();
        menuOpen = GameMenuOpen.Normal;
        player.ToogleControlls();
    }

    public void Close()
    {
        Time.timeScale = 1;
        background.SetActive(false);
        pauseMenu.Close();
        player.ToogleControlls();
        menuOpen = GameMenuOpen.None;
    }

    public void OpenSettings()
    {
        menuOpen = GameMenuOpen.Settings;
        Settings.Open();
    }

    public void CloseSettings()
    {
        menuOpen = GameMenuOpen.Normal;
        Settings.Close();
    }
}
