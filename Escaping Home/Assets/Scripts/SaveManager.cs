using System.IO;
using UnityEngine;

public static class SaveManager
{
    // Localtion: %appdata%\..\LocalLow\DefaultCompany\Escaping Home
    private static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");
    public static bool SaveFileExists() => File.Exists(SavePath);

    public static void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
    }

    public static void SaveCollectable(SceneNames collectableLevel)
    {
        GameData data = Load();
        if (data.Collectables.Contains(collectableLevel))
            return;

        data.Collectables.Add(collectableLevel);
        Save(data);
    }

    public static GameData Load()
    {
        if (!SaveFileExists())
        {
            GameData data = new(); // default
            Save(data);
            return data;
        }

        string json = File.ReadAllText(SavePath);
        GameData gameData = JsonUtility.FromJson<GameData>(json);
        return gameData ?? new();
    }

    public static void DeleteSave()
    {
        if (SaveFileExists())
            File.Delete(SavePath);
    }
}
