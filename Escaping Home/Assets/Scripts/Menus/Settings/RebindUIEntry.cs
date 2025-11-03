[System.Serializable] // This is to make it able to be editable in a list :)
public class RebindUIEntry
{
    public string Name;
    public string actionPath = "Player/";
    public int bindingIndex = 0;
    public RebindUIEntryBtn btn;
}
