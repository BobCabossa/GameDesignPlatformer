using System;

public static class Level
{
    /// <summary>
    /// Removes the menus and none main level form the enum 
    /// </summary>
    public static int Calculate(SceneNames scene) => (int)scene - 3;

    public static void IfBeatHighestSave()
    {
        GameData gameData = SaveManager.Load();
        string levelName = SceneLoader.GetSceneName();

        if (!Enum.TryParse(levelName, out SceneNames sceneName))
            return;

        // Addes 1 to level to unlock the next level
        int level = Calculate(sceneName) + 1;
        if (level > gameData.highestLevelBeat)
        {
            gameData.highestLevelBeat = level;
            SaveManager.Save(gameData);
        }
    }
}
