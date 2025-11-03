using System.Collections.Generic;
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
            int level = Level.Calculate(button.LevelName);
            bool unlocked = level <= gameData.highestLevelBeat;
            button.gameObject.SetActive(unlocked);
        }
    }
}
