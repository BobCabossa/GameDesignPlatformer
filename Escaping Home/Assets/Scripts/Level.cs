using UnityEngine;

public static class Level
{
    private const int ScenesBeforeLevels = 3;
    private const SceneNames HighestLevel = SceneNames.LevelTwo;

    /// <summary>
    /// Removes the menus and none main level form the enum 
    /// </summary>
    public static int Calculate(SceneNames scene) => (int)scene - ScenesBeforeLevels;

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
        int sceneIndex = gameData.highestLevelBeat + ScenesBeforeLevels - 1;
        SceneNames levelNames = SceneNameHelper.GetSceneName(sceneIndex);
        return levelNames == HighestLevel;
    }
}
