using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindingUI : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField]
    private InputActionAsset inputActionsAsset;

    [Header("Bindings to Manage")]
    [SerializeField]
    private List<RebindUIEntry> rebindEntries = new();

    private readonly Dictionary<InputAction, string> _originalBindingPaths = new();
    private InputActionRebindingExtensions.RebindingOperation rebindOperation;
    private MainMenuController mainMenuController;
    private Player player;

    private void Awake()
    {
        LoadRebinds();
        InitializeUI();

        mainMenuController = FindAnyObjectByType<MainMenuController>();
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
        int index = entry.actionBindingIndex;
        if (action == null)
        {
            Debug.LogError($"Action '{entry.actionPath}' not found in asset!");
            return;
        }

        _originalBindingPaths[action] = action.bindings[index].effectivePath;
        entry.btn.bindingDisplayText.text = "Press any key...";
        action.Disable();

        rebindOperation = action.PerformInteractiveRebinding(index)
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(op => RebindComplete(entry, action))
            .Start();
    }

    private void RebindComplete(RebindUIEntry entry, InputAction action)
    {
        InputControl control = rebindOperation.selectedControl;
        rebindOperation.Dispose();
        action.Enable();

        try
        {
            CheckToSwap(control, entry, action);
            MirrorRebindToUI(entry, action);
            SaveRebinds();
            LoadRebinds();
            InitializeUI();
        }
        finally
        {
            _originalBindingPaths.Remove(action);
        }
    }

    private void CheckToSwap(InputControl control, RebindUIEntry entry, InputAction action)
    {
        string newPath = control.path;

        // Check within the same composite (Need to check before "RebindingValidator")
        CheckToSwapComposite(action, entry.actionBindingIndex, newPath);

        // Check global
        if (RebindingValidator.IsControlAlreadyUsed(inputActionsAsset, control, action,
            out InputAction conflictAction, out int conflictIndex))
        {
            SwapGlobally(entry, action, conflictAction, conflictIndex);
        }

        // Clean up any duplicates after swap
        RemoveDuplicateBindings(control, action, entry.actionBindingIndex);
    }

    private void RemoveDuplicateBindings(InputControl control, InputAction currentAction, int currentIndex)
    {
        string controlPath = NormalizeBindingPath(control.path);

        foreach (var action in inputActionsAsset)
        {
            for (int i = 0; i < action.bindings.Count; i++)
            {
                if (action == currentAction && i == currentIndex)
                    continue;

                string path = action.bindings[i].effectivePath;
                string normalizedPath = NormalizeBindingPath(path);

                if (normalizedPath == controlPath)
                    action.ApplyBindingOverride(i, string.Empty);
            }
        }
    }

    private void SwapGlobally(RebindUIEntry entry, InputAction action, InputAction conflictAction, int conflictIndex)
    {
        string conflictPath = conflictAction.bindings[conflictIndex].effectivePath;
        if (!_originalBindingPaths.TryGetValue(action, out string oldPath))
            oldPath = action.bindings[entry.actionBindingIndex].effectivePath;

        action.ApplyBindingOverride(entry.actionBindingIndex, conflictPath);
        conflictAction.ApplyBindingOverride(conflictIndex, oldPath);
    }

    private void CheckToSwapComposite(InputAction action, int bindingIndex, string newPath)
    {
        var currentBinding = action.bindings[bindingIndex];
        if (!currentBinding.isPartOfComposite)
            return;

        string normalizedNewPath = NormalizeBindingPath(newPath);

        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (i == bindingIndex)
                continue;

            var otherBinding = action.bindings[i];
            if (!otherBinding.isPartOfComposite)
                continue;

            // Ensure they're part of the same composite (same root)
            if (GetCompositeRootIndex(action, otherBinding) != GetCompositeRootIndex(action, currentBinding))
                continue;

            string otherPath = otherBinding.effectivePath;
            string normalizedOtherPath = NormalizeBindingPath(otherPath);

            if (string.Equals(normalizedNewPath, normalizedOtherPath, System.StringComparison.OrdinalIgnoreCase))
            {
                if (_originalBindingPaths.TryGetValue(action, out string keyBind))
                    action.ApplyBindingOverride(i, keyBind);
                else
                    action.ApplyBindingOverride(i, string.Empty);
            }
        }
    }

    private string NormalizeBindingPath(string path)
    {
        if (string.IsNullOrEmpty(path))
            return path;

        path = path.Trim();

        // That we wants
        if (path.StartsWith("<"))
            return path; // already in desired form

        // Convert "/Keyboard/a" -> "<Keyboard>/a"
        if (path.StartsWith("/"))
        {
            string withoutLeadingSlash = path.Substring(1);
            int slashIndex = withoutLeadingSlash.IndexOf('/');
            if (slashIndex > 0)
            {
                string device = withoutLeadingSlash.Substring(0, slashIndex);
                string key = withoutLeadingSlash[slashIndex..];
                return $"<{device}>{key}";
            }
        }

        // As a fallback, return as-is
        return path;
    }

    private int GetCompositeRootIndex(InputAction action, InputBinding binding)
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (action.bindings[i].isComposite)
            {
                int rootIndex = i;
                for (int j = i + 1; j < action.bindings.Count; j++)
                {
                    if (!action.bindings[j].isPartOfComposite)
                        break;

                    if (action.bindings[j].name == binding.name)
                        return rootIndex;
                }
            }
        }

        return -1;
    }

    private void MirrorRebindToUI(RebindUIEntry entry, InputAction action)
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
        PlayerPrefs.Save();
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

        if (player != null)
        {
            player.Controls.RemoveAllBindingOverrides();
        }
        else if (mainMenuController != null)
        {
            mainMenuController.Controls.RemoveAllBindingOverrides();
        }

        PlayerPrefs.DeleteKey("rebinds");
        PlayerPrefs.Save();
        InitializeUI();
    }
}
