using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public RectTransform WinText;
    public RectTransform WinThankYouText;
    public RectTransform HintText;

    [Header("Won level")]
    public float growthSpeedNormal = 500f; // units per second
    public float delayBeforeGoingBackNormal = 1;

    [Header("Won game")]
    public float growthSpeedOnWinGame = 250f;
    public float delayBeforeGoingBackWinGame = 10;

    public bool notLoading = true;

    private Vector2 winOriginalSize;
    private Vector2 winThankYouOriginalSize;
    private Vector2 hintOriginalSize;

    private bool ExtraGrow = false;
    private bool growing = false;

    private float growthSpeed = 1;
    private float delayBeforeGoingBack = 1;

    private void Awake()
    {
        SceneNames level = SceneLoader.GetSceneName();
        bool IsHighestLevel = Level.IsHighestLevel(level);

        ExtraGrow = IsHighestLevel;
        if (IsHighestLevel)
        {
            growthSpeed = growthSpeedOnWinGame;
            delayBeforeGoingBack = delayBeforeGoingBackWinGame;
        }
        else
        {
            growthSpeed = growthSpeedNormal;
            delayBeforeGoingBack = delayBeforeGoingBackNormal;

            WinThankYouText.gameObject.SetActive(false);
            HintText.gameObject.SetActive(false);

            WinThankYouText.sizeDelta = Vector2.one;
            HintText.sizeDelta = Vector2.one;
        }
    }

    private void Start()
    {
        // Store original size
        winOriginalSize = WinText.sizeDelta;

        // Shrink to 1,1
        WinText.sizeDelta = Vector2.one;

        if (ExtraGrow)
        {
            winThankYouOriginalSize = WinThankYouText.sizeDelta;
            hintOriginalSize = HintText.sizeDelta;

            WinThankYouText.sizeDelta = Vector2.one;
            HintText.sizeDelta = Vector2.one;
        }

        growing = true;
    }

    private void FixedUpdate()
    {
        if (growing)
        {
            // Move current size towards original size smoothly
            WinText.sizeDelta = Vector2.MoveTowards(WinText.sizeDelta, winOriginalSize, growthSpeed * Time.deltaTime);

            if (ExtraGrow)
            {
                WinThankYouText.sizeDelta = Vector2.MoveTowards(WinThankYouText.sizeDelta, winThankYouOriginalSize, growthSpeed * Time.deltaTime);
                HintText.sizeDelta = Vector2.MoveTowards(HintText.sizeDelta, hintOriginalSize, growthSpeed * Time.deltaTime);
            }

            // Stop when we reach original size
            if (IsGrowing())
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

    private bool IsGrowing()
    {
        bool extra = true;
        if (ExtraGrow)
        {
            extra = WinThankYouText.sizeDelta == winThankYouOriginalSize
                && HintText.sizeDelta == hintOriginalSize;
        }

        return WinText.sizeDelta == winOriginalSize && extra;
    }
}
