using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int highestLevelBeat = 0;
    public List<SceneNames> Collectables = new();
}
