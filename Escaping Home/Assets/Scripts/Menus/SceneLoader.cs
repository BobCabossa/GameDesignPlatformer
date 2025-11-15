using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class SceneLoader : MonoBehaviour
{
    private static SceneNames SceneToLoad = SceneNames.None;
    public RectTransform SpinningThing;
    public float rotationSpeed = 5;

    [Space(10)]
    public TextMeshProUGUI progressText;
    public Slider progressBar;

    [Space(10)]
    public GameObject canvas;
    public AudioListener audioListener;

    public static void SetSceneToActiveScene() => SceneToLoad = SceneNameHelper.GetSceneName(SceneManager.GetActiveScene().name);
    public static bool FirstLoad() => SceneToLoad == SceneNames.None;
    public static SceneNames GetSceneName() => SceneToLoad;

    public static async void LoadScene(SceneNames sceneName)
    {
        SceneToLoad = sceneName;

        // Load loading scene additively so we don't block
        AsyncOperation loadingSceneOp = SceneManager.LoadSceneAsync(
            SceneNames.LoadingScene.ToString(), LoadSceneMode.Additive);

        while (!loadingSceneOp.isDone)
            await Task.Yield();
    }

    public static void LoadPreviousScene()
    {
        if (SceneToLoad == SceneNames.None)
        {
            Debug.LogWarning("A scene tried to be loaded without a scene name.");
            return;
        }

        LoadScene(SceneToLoad);
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
        if (SceneToLoad == SceneNames.None)
            return;

        Time.timeScale = 1;
        await LoadAsyncScene();
    }

    private async Task LoadAsyncScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneToLoad.ToString());
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

        await Task.Delay(250);

        if (audioListener != null)
            audioListener.enabled = false;

        operation.allowSceneActivation = true;

        while (!operation.isDone)
            await Task.Yield();

        //await SceneManager.UnloadSceneAsync("LoadingScene");
    }

    private void Update()
    {
        if (SpinningThing != null)
            SpinningThing.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
