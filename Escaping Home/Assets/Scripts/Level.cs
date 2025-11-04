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

    public static bool BeatAllLevels()
    {
        GameData gameData = SaveManager.Load();

        // Minus 1 get it to line up, from save.
        int sceneIndex = gameData.highestLevelBeat - 1;
        SceneNames levelNames = SceneNameHelper.GetSceneName(sceneIndex);
        return levelNames == SceneNameHelper.HighestLevel;
    }
}
