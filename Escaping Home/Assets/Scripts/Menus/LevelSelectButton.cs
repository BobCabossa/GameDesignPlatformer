using UnityEngine;

public class LevelSelectButton : MonoBehaviour
{
    public ScreneNames LevelName = ScreneNames.TestLevel;

    public void StartLevel()
    {
        SceneLoader.LoadScene(LevelName);
    }
}
