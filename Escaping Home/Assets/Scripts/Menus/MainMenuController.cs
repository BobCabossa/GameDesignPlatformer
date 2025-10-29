using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject StartMenu;
    public GameObject LevelSelectorMenu;

    private void Awake()
    {
        bool firstLoad = SceneLoader.FirstLoad();
        StartMenu.SetActive(firstLoad);
        LevelSelectorMenu.SetActive(!firstLoad);
    }

    public void StartGame()
    {
        StartMenu.SetActive(false);
        LevelSelectorMenu.SetActive(true);
    }

    public void BackToMainMenu()
    {
        LevelSelectorMenu.SetActive(false);
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
