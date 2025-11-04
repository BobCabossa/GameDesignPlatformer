using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    [SerializeField]
    private LevelSelectButton[] buttonList;

    private void Awake()
    {
        buttonList = GetComponentsInChildren<LevelSelectButton>();

        InitializeUI();
    }

    public void Open()
    {
        gameObject.SetActive(true);
        InitializeUI();
    }

    private void InitializeUI()
    {
        GameData gameData = SaveManager.Load();

        foreach (LevelSelectButton button in buttonList)
        {
            int level = (int)button.LevelName;
            bool unlocked = level <= gameData.highestLevelBeat;

            // To normal levels
            if (button.LevelName != SceneNames.TestLevel)
            {
                button.gameObject.SetActive(unlocked);
                continue;
            }

            // Only unlock if beat all levels
            bool unlockedEasterEgg = Level.UnlockedEsterEggLevel(gameData.Collectables);
            button.gameObject.SetActive(unlockedEasterEgg);
        }
    }
}
