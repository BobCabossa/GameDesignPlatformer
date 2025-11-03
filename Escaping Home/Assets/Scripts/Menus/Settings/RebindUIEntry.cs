using UnityEngine;

[System.Serializable] // This is to make it able to be editable in a list :)
public class RebindUIEntry
{
    [Tooltip("Doesn't do enything in the code, only gives the item the name")]
    public string Name;

    [Space(5)]
    [Tooltip("Path to the action e.g. \"Player/move\"")]
    public string actionPath = "Player/";
    [Tooltip("What item to take from the path")]
    public int actionBindingIndex = 0;

    [Space(5)]
    [Tooltip("Leave this empty if there are no ui action on this key")]
    public string uiPath = "UI/";
    [Tooltip("If no path set ignore this")]
    public int uiBindingIndex = 0;

    [Space(5)]
    public RebindUIEntryBtn btn;
}
