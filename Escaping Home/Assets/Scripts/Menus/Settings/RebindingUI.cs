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

    private void Awake()
    {
        LoadRebinds();
        InitializeUI();
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
            entry.btn.bindingDisplayText.text = GetBindingDisplayName(action, entry.bindingIndex);

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

        rebindOperation = action.PerformInteractiveRebinding(entry.bindingIndex)
            .WithControlsExcluding("Mouse") // optional
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(op => RebindComplete(entry, action))
            .Start();
    }

    private void RebindComplete(RebindUIEntry entry, InputAction action)
    {
        rebindOperation.Dispose();
        action.Enable();

        entry.btn.bindingDisplayText.text = GetBindingDisplayName(action, entry.bindingIndex);
        SaveRebinds();
    }

    private void SaveRebinds()
    {
        string rebinds = inputActionsAsset.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }

    private void LoadRebinds()
    {
        if (PlayerPrefs.HasKey("rebinds"))
        {
            string rebinds = PlayerPrefs.GetString("rebinds");
            inputActionsAsset.LoadBindingOverridesFromJson(rebinds);
        }
    }

    public void ResetAllRebinds()
    {
        inputActionsAsset.RemoveAllBindingOverrides();
        PlayerPrefs.DeleteKey("rebinds");
        InitializeUI();
    }
}
