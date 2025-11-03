using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class RebindingUI : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField]
    private InputActionAsset inputActionsAsset;

    [Header("Bindings to Manage")]
    [SerializeField]
    private List<RebindUIEntry> rebindEntries = new();

    private InputActionRebindingExtensions.RebindingOperation rebindOperation;
    private Player player;

    private void Awake()
    {
        LoadRebinds();
        InitializeUI();

        player = FindAnyObjectByType<Player>();
    }

    private void InitializeUI()
    {
        foreach (var entry in rebindEntries)
        {
            InputAction action = inputActionsAsset.FindAction(entry.actionPath);
            if (action == null)
            {
                Debug.LogError($"Action '{entry.actionPath}' not found in asset!");
                continue;
            }

            // Update label
            entry.btn.bindingDisplayText.text = GetBindingDisplayName(action, entry.actionBindingIndex);

            // Add button listener
            entry.btn.rebindButton.onClick.RemoveAllListeners();
            entry.btn.rebindButton.onClick.AddListener(() => StartRebind(entry));
        }
    }

    private string GetBindingDisplayName(InputAction action, int bindingIndex)
    {
        if (bindingIndex < 0 || bindingIndex >= action.bindings.Count)
            return "N/A";

        return InputControlPath.ToHumanReadableString(
            action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    private void StartRebind(RebindUIEntry entry)
    {
        var action = inputActionsAsset.FindAction(entry.actionPath);
        if (action == null)
        {
            Debug.LogError($"Action '{entry.actionPath}' not found in asset!");
            return;
        }

        entry.btn.bindingDisplayText.text = "Press any key...";
        action.Disable();

        rebindOperation = action.PerformInteractiveRebinding(entry.actionBindingIndex)
            .WithControlsExcluding("Mouse") // optional
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(op => RebindComplete(entry, action))
            .Start();
    }

    private void RebindComplete(RebindUIEntry entry, InputAction action)
    {
        rebindOperation.Dispose();
        action.Enable();

        MirrorRebind(entry, action);
        SaveRebinds();
        LoadRebinds();

        entry.btn.bindingDisplayText.text = GetBindingDisplayName(action, entry.actionBindingIndex);
    }

    private void MirrorRebind(RebindUIEntry entry, InputAction action)
    {
        if (string.IsNullOrEmpty(entry.uiPath))
            return;

        var uiAction = inputActionsAsset.FindAction(entry.uiPath);
        var path = action.bindings[entry.uiBindingIndex].effectivePath;
        uiAction.ApplyBindingOverride(entry.uiBindingIndex, path);
    }

    private void SaveRebinds()
    {
        string rebinds = inputActionsAsset.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }

    private void LoadRebinds()
    {
        if (!PlayerPrefs.HasKey("rebinds"))
            return;

        string rebinds = PlayerPrefs.GetString("rebinds");
        inputActionsAsset.LoadBindingOverridesFromJson(rebinds);
        if (player != null)
        {
            player.Controls.LoadBindingOverridesFromJson(rebinds);
        }
    }

    public void ResetAllRebinds()
    {
        inputActionsAsset.RemoveAllBindingOverrides();
        PlayerPrefs.DeleteKey("rebinds");
        InitializeUI();
    }
}
