using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScreenSettings : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Dropdown resolutionDropdown;
    public RectTransform resolutionRectTransform;

    [Space(5)]
    public TMP_Dropdown displayModeDropdown;
    public RectTransform displayModeRectTransform;

    [Space(5)]
    public TMP_Dropdown refreshRateDropdown;
    public RectTransform refreshRateRectTransform;

    private Resolution[] resolutions;

    private bool IsAspectRatio(float width, float height, float aspectW, float aspectH, float tolerance = 0.01f)
    {
        return Mathf.Abs((width / height) - (aspectW / aspectH)) < tolerance;
    }

    private void Awake()
    {
        resolutions = Screen.resolutions
            .Where(r => IsAspectRatio(r.width, r.height, 16f, 9f))
            .OrderByDescending(r => r.width)
            .ThenByDescending(r => r.refreshRateRatio.numerator / (float)r.refreshRateRatio.denominator)
            .ToArray();

        displayModeDropdown.ClearOptions();
        displayModeDropdown.AddOptions(new List<string>
        {
            "Fullscreen",
            "Borderless Window",
            "Windowed"
        });

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutions
            .Select(r => $"{r.width} x {r.height}")
            .Distinct()
            .ToList());

        refreshRateDropdown.ClearOptions();
        refreshRateDropdown.AddOptions(resolutions
            .Select(r => $"{r.refreshRateRatio.numerator / (float)r.refreshRateRatio.denominator:0.##} Hz")
            .Distinct()
            .ToList());

        if (PlayerPrefs.HasKey("ResolutionIndex"))
        {
            resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex"); ;
            displayModeDropdown.value = PlayerPrefs.GetInt("DisplayMode"); ;
            refreshRateDropdown.value = PlayerPrefs.GetInt("RefreshRateIndex");
            ApplySettings();
        }
    }

    public void ApplySettings()
    {
        var res = resolutions[resolutionDropdown.value];

        // Parse refresh rate
        string hzText = refreshRateDropdown.options[refreshRateDropdown.value].text
            .Replace(" Hz", "").Trim();

        if (float.TryParse(hzText, NumberStyles.Float, CultureInfo.InvariantCulture, out float hz))
        {
            Debug.LogWarning($"⚠️ Could not parse refresh rate from '{hzText}', defaulting to 60 Hz.");
            hz = 60f;
        }

        //float hz = float.Parse(refreshRateDropdown.options[refreshRateDropdown.value].text.Replace(" Hz", ""));
        RefreshRate refresh = new()
        {
            numerator = (uint)(hz * 1000),
            denominator = 1000
        };

        var mode = displayModeDropdown.value switch
        {
            0 => FullScreenMode.ExclusiveFullScreen,
            1 => FullScreenMode.FullScreenWindow,
            _ => FullScreenMode.Windowed
        };

        Screen.SetResolution(res.width, res.height, mode, refresh);
        SaveSettings();
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
        PlayerPrefs.SetInt("DisplayMode", displayModeDropdown.value);
        PlayerPrefs.SetInt("RefreshRateIndex", refreshRateDropdown.value);
        PlayerPrefs.Save();
    }
}
