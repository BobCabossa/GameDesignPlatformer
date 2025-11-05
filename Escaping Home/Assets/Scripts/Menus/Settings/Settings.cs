using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> menues = new();
    private bool needReactivation = false;

    private void Start()
    {
        SceneNames sceneName = SceneLoader.GetSceneName();
        if (sceneName == SceneNames.MainMenu)
        {
            needReactivation = true;
            foreach (var item in menues)
            {
                item.SetActive(false);
            }
        }
    }

    // This is only to load setting in the beginning of the games lifetime
    public void Open()
    {
        if (needReactivation)
        {
            needReactivation = false;
            foreach (var item in menues)
            {
                item.SetActive(true);
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
