using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelSelectButton : MonoBehaviour
{
    public string LevelTitle;
    public SceneNames LevelName = SceneNames.TestLevel;

    [Space(5)]
    public List<SceneNames> Requirement = new();

    [Space(10)]
    public TextMeshProUGUI titleText;

    public void StartLevel()
    {
        SceneLoader.LoadScene(LevelName);
    }

    private void OnValidate()
    {
        string levelTitle = LevelTitle;
        if (string.IsNullOrWhiteSpace(LevelTitle))
            levelTitle = PauseMenu.AddSpacesToSentence(LevelName.ToString());

        if (titleText != null)
            titleText.text = levelTitle;
    }
}
