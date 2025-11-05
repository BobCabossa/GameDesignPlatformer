using System;
using System.Linq;

public enum SceneNames
{
    // Don't use: it will make errors
    // As it's only use, is as a defualt before it's asigned in a method or inspector.
    None = -99, 

    // Menues, max: 48
    MainMenu,
    LoadingScene,

    // Easter eggs, max: 49
    TestLevel = -50,

    // Levels, max: unlimited
    LevelOne = 0,
    LevelTwo,
    LevelThree,
    LevelFour,
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
