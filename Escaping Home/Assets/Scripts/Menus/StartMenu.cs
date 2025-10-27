using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneLoader.LoadScene(ScreneNames.TestLevel.ToString());
    }

    public void ExitGame()
    {
        Quit();
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
