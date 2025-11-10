using System;
using System.Linq;

public enum SceneNames
{
    // Don't use, only as default
    None = -99, 

    // Menues
    MainMenu,
    LoadingScene,

    // Easter eggs
    TestLevel = -50,

    // Levels
    LevelOne = 0,
    LevelTwo,
    LevelThree,
    LevelFour,
    LevelFive,
    LevelSix,
    LevelSeven,
    LevelEight,
    LevelNine,
}

public static class SceneNameHelper
{
    public static SceneNames HighestLevel
    {
        get => Enum.GetValues(typeof(SceneNames))
                 .Cast<SceneNames>()
                 .Max();
    }

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
