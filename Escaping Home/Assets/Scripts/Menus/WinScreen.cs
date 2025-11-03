using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public RectTransform WinText;
    private Vector2 originalSize;
    private bool growing = false;
    public float growthSpeed = 500f; // units per second
    public float delayBeforeGoingBack = 1;
    public bool notLoading = true;

    private void Start()
    {
        originalSize = WinText.sizeDelta; // store original size
        WinText.sizeDelta = Vector2.one; // shrink to 1,1
        growing = true;
    }

    private void FixedUpdate()
    {
        if (growing)
        {
            // Move current size towards original size smoothly
            WinText.sizeDelta = Vector2.MoveTowards(WinText.sizeDelta, originalSize, growthSpeed * Time.deltaTime);

            // Stop when we reach original size
            if (WinText.sizeDelta == originalSize)
                growing = false;
        }
        else if (notLoading)
        {
            delayBeforeGoingBack -= Time.deltaTime;
            if (delayBeforeGoingBack <= 0)
            {
                notLoading = false;
                SceneLoader.LoadScene(SceneNames.MainMenu);
            }
        }
    }
}
