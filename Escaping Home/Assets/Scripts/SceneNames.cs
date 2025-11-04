using System;

public enum SceneNames
{
    MainMenu,
    LoadingScene,

    TestLevel, // Keep as a easter egg.

    // Levels
    LevelOne,
    LevelTwo,
}

public static class SceneNameHelper
{
    public static SceneNames GetSceneName(string sceneName)
    {
        if (Enum.TryParse(sceneName, out SceneNames levelName))
            return levelName;

        return SceneNames.MainMenu;
    }

    public static SceneNames GetSceneName(int sceneIndex)
    {
        if (Enum.IsDefined(typeof(SceneNames), sceneIndex))
            return (SceneNames)sceneIndex;

        return SceneNames.MainMenu;
    }
}
