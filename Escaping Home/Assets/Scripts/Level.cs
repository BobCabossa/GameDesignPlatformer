using UnityEngine;

public static class Level
{
    /// <summary>
    /// Removes the menus and none main level form the enum 
    /// </summary>
    public static int Calculate(SceneNames scene) => (int)scene - SceneNameHelper.ScenesBeforeLevels;

    public static void IfBeatHighestSave()
    {
        GameData gameData = SaveManager.Load();
        SceneNames levelName = SceneLoader.GetSceneName();

        // Addes 1 to level to unlock the next level
        int level = Calculate(levelName) + 1;
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
        int sceneIndex = gameData.highestLevelBeat + SceneNameHelper.ScenesBeforeLevels - 1;
        SceneNames levelNames = SceneNameHelper.GetSceneName(sceneIndex);
        return levelNames == SceneNameHelper.HighestLevel;
    }
}
