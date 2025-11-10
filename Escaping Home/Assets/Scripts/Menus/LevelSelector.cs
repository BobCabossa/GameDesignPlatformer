using System.Collections.Generic;
using System.Linq;
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
            if (button.Requirement.Count > 0)
            {
                bool unlockedEasterEgg = RequirementMeet(gameData.Collectables, button.Requirement);
                button.gameObject.SetActive(unlockedEasterEgg);
                continue;
            }

            int level = (int)button.LevelName;
            bool unlocked = level <= gameData.highestLevelBeat;
            button.gameObject.SetActive(unlocked);
        }
    }

    private bool RequirementMeet(List<SceneNames> levels, List<SceneNames> requirement)
    {
        return !requirement.Except(levels).Any();
    }
}
