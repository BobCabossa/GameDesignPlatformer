using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject StartMenu;
    public Settings Settings;
    public LevelSelector LevelSelectorMenu;

    private void Awake()
    {
        // Need the settings to be active to load setting.
        Settings.gameObject.SetActive(true);

        bool firstLoad = SceneLoader.FirstLoad();
        if (firstLoad)
            SceneLoader.SetSceneToActiveScene();

        StartMenu.SetActive(firstLoad);

        LevelSelectorMenu.gameObject.SetActive(!firstLoad);
        if (!firstLoad)
            LevelSelectorMenu.Open();
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
