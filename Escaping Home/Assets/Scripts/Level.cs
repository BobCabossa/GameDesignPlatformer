using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Level
{
    public static void IfBeatHighestSave()
    {
        GameData gameData = SaveManager.Load();
        SceneNames levelName = SceneLoader.GetSceneName();

        // Addes 1 to level to unlock the next level
        int level = (int)levelName + 1;
        if (level > gameData.highestLevelBeat)
        {
            gameData.highestLevelBeat = level;
            SaveManager.Save(gameData);
        }
    }

    public static bool UnlockedEsterEggLevel(List<SceneNames> levels)
    {
        SceneNames[] requirement = new SceneNames[] {
            SceneNames.LevelOne,
            SceneNames.LevelTwo,
            SceneNames.LevelThree,
        };

        return !requirement.Except(levels).Any();
    }
}
