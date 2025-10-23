using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public void StartGame(string sceneName)
    {
        SceneLoader.LoadScene(sceneName);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
