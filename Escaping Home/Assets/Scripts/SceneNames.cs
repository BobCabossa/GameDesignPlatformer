using System;

public enum SceneNames
{
    None, // Don't use, in game, it will make errors
    MainMenu,
    LoadingScene,

    TestLevel, // Keep as a easter egg.

    // Levels
    LevelOne,
    LevelTwo,
}

public static class SceneNameHelper
{
    public const int ScenesBeforeLevels = 4;
    public const SceneNames HighestLevel = SceneNames.LevelTwo;

    public static SceneNames GetSceneName(string sceneName)
    {
        if (Enum.TryParse(sceneName, out SceneNames levelName))
            return levelName;

        return SceneNames.None;
    }

    public static SceneNames GetSceneName(int sceneIndex)
    {
        if (Enum.IsDefined(typeof(SceneNames), sceneIndex))
            return (SceneNames)sceneIndex;

        return SceneNames.None;
    }
}
