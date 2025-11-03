using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    public static void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"Saved to {SavePath}");
    }

    public static GameData Load()
    {
        if (!File.Exists(SavePath))
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
        if (File.Exists(SavePath))
            File.Delete(SavePath);
    }
}
