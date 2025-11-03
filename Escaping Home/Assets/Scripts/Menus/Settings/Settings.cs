using UnityEngine;

public class Settings : MonoBehaviour
{
    public void RestSave()
    {
        SaveManager.DeleteSave();
    }
}
