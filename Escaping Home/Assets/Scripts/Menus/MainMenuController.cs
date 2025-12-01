using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public InputSystemActions Controls;

    public GameObject StartMenu;
    public Settings Settings;
    public LevelSelector LevelSelectorMenu;

    public Button continueBtn;

    // When the scene is destroyed by scene change
    private void OnDestroy() => Controls?.Dispose();
    
    private void Awake()
    {
        // Need the settings to be active to load setting.
        Settings.gameObject.SetActive(true);

        bool firstLoad = SceneLoader.FirstLoad();
        if (firstLoad)
            SceneLoader.SetSceneToActiveScene();

        CheckSaveFile();
        SetMenuToOpen(firstLoad);
        UIControls();
    }

    private void CheckSaveFile()
    {
        continueBtn.interactable = SaveManager.SaveFileExists();
    }

    private void SetMenuToOpen(bool firstLoad)
    {
        StartMenu.SetActive(firstLoad);

        LevelSelectorMenu.gameObject.SetActive(!firstLoad);
        if (!firstLoad)
            LevelSelectorMenu.Open();
    }

    private void UIControls()
    {
        Controls = new();
        Controls.UI.Enable();
        Controls.Player.Disable();

        Controls.UI.Close.performed += _ => BackToMainMenu();
    }

    public void StartNewGame()
    {
        SaveManager.Save(new());
        StartGame();
    }

    public void StartGame()
    {
        StartMenu.SetActive(false);
        LevelSelectorMenu.Open();
    }

    public void OpenSettings()
    {
        StartMenu.SetActive(false);
        Settings.Open();
    }

    public void BackToMainMenu()
    {
        CheckSaveFile();
        Settings.gameObject.SetActive(false);
        LevelSelectorMenu.gameObject.SetActive(false);
        StartMenu.SetActive(true);
    }

    public static void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
