using TMPro;
using UnityEngine;
using UnityEngine.UI;

//[System.Serializable] // This is to make it able to be editable in a list :)
public class RebindUIEntry : MonoBehaviour
{
    public string actionPath = "Player/";
    public int bindingIndex = 0;
    public Button rebindButton;
    public TextMeshProUGUI bindingDisplayText;
}
