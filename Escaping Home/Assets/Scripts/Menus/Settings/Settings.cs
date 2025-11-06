using UnityEngine;

public class Settings : MonoBehaviour
{
    private bool needReactivation = false;

    private void Start()
    {
        SceneNames sceneName = SceneLoader.GetSceneName();
        if (sceneName == SceneNames.MainMenu)
        {
            needReactivation = true;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    // This is only to load setting in the beginning of the games lifetime
    public void Open()
    {
        if (needReactivation)
        {
            needReactivation = false;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        else
            gameObject.SetActive(true);
    }

    public void ResetSave()
    {
        SaveManager.DeleteSave();
    }
}
