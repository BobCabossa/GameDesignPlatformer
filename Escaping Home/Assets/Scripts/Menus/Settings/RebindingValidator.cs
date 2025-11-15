using UnityEngine;
using UnityEngine.InputSystem;

public static class RebindingValidator
{
    private static string NormalizePathString(string path)
    {
        if (string.IsNullOrEmpty(path))
            return path;

        // Remove angle brackets and whitespace, ensure a leading slash, lowercase
        string s = path.Trim();
        s = s.Replace(" ", "");     // remove spaces
        s = s.Replace("<", "");     // remove '<'
        s = s.Replace(">", "");     // remove '>'
        if (!s.StartsWith("/"))
            s = "/" + s;

        return s.ToLowerInvariant();
    }

    private static bool BindingContainsControlPath(string bindingPath, string controlPath)
    {
        if (string.IsNullOrEmpty(bindingPath))
            return false;

        // Some binding strings use '|' to separate alternatives or use ',' in some cases.
        // Split on '|' and ',' to check each alternative individually.
        var parts = bindingPath.Split(new char[] { '|', ',' }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (var part in parts)
        {
            string normPart = NormalizePathString(part);
            if (normPart == controlPath)
                return true;
        }

        return false;
    }

    public static bool IsControlAlreadyUsed(
        InputActionAsset asset, InputControl newControl, InputAction currentAction,
        out InputAction conflictAction, out int conflictIndex)
    {
        conflictAction = null;
        conflictIndex = -1;
        if (asset == null || newControl == null)
            return false;

        string newControlPath = NormalizePathString(newControl.path);

        foreach (var map in asset.actionMaps)
        {
            foreach (var action in map.actions)
            {
                if (action == currentAction)
                    continue;

                for (int i = 0; i < action.bindings.Count; i++)
                {
                    var binding = action.bindings[i];
                    if (binding.isComposite)
                        continue;

                    // binding.effectivePath may contain multiple alternatives
                    string bindingPath = binding.effectivePath;

                    if (BindingContainsControlPath(bindingPath, newControlPath))
                    {
                        conflictAction = action;
                        conflictIndex = i;
                        return true;
                    }
                }
            }
        }

        return false;
    }
}

