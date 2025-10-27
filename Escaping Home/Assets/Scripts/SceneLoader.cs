using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    private static string SceneToLoad = string.Empty;
    public RectTransform SpinningThing;
    public float rotationSpeed = 5;

    [Space(10)]
    public TextMeshProUGUI progressText;
    public Slider progressBar;

    [Space(10)]
    public GameObject canvas;
    public AudioListener audioListener;

    public static async void LoadScene(string sceneName)
    {
        SceneToLoad = sceneName;

        // Load loading scene additively so we don't block
        AsyncOperation loadingSceneOp = SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);
        while (!loadingSceneOp.isDone)
            await Task.Yield();
    }

    public static void LoadPreviousScene()
    {
        if (SceneToLoad == string.Empty)
        {
            Debug.LogWarning("A scene tried to be loaded without a scene name.");
            return;
        }
        
        SceneManager.LoadScene("LoadingScene");
    }

    private void Awake()
    {
        var listeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        foreach (var listener in listeners)
        {
            listener.enabled = false;
        }
    }

    private async void Start()
    {
        // Only start if we have a target scene
        if (string.IsNullOrEmpty(SceneToLoad))
            return;

        await LoadAsyncScene();
    }

    private async Task LoadAsyncScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneToLoad);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (progressBar != null)
                progressBar.value = progress;

            if (progressText != null)
                progressText.text = (progress * 100f).ToString("F0") + "%";

            await Task.Yield();
        }

        // Give the spinner a bit of time to show
        await Task.Delay(250);

        audioListener.enabled = false;

        // Activate the new scene
        operation.allowSceneActivation = true;

        // Wait until it's loaded
        while (!operation.isDone)
            await Task.Yield();

        // Unload the loading scene
        canvas.SetActive(false);
        await SceneManager.UnloadSceneAsync("LoadingScene");
    }

    private void Update()
    {
        if (SpinningThing != null)
            SpinningThing.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
