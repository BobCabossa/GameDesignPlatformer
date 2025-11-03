using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject StartMenu;
    public GameObject Settings;
    public LevelSelector LevelSelectorMenu;

    private void Awake()
    {
        bool firstLoad = SceneLoader.FirstLoad();
        Settings.SetActive(false);
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
        Settings.SetActive(true);
    }

    public void BackToMainMenu()
    {
        Settings.SetActive(false);
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
