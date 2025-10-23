using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    private static string SceneToLoad = string.Empty;

    public RectTransform SpinningThing;
    public float rotationSpeed = 5;

    [Space(10)]
    public Text progressText;
    public Slider progressBar;

    public static void LoadScene(string sceneName)
    {
        SceneToLoad = sceneName;
        LoadPreviousScene();
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

    private void Start()
    {
        // Automatically start loading the next scene
        LoadAsyncScene();
    }

    private async void LoadAsyncScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneToLoad);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // Calculate progress (goes from 0 to 0.9)
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (progressBar != null)
                progressBar.value = progress;

            if (progressText != null)
                progressText.text = (progress * 100f).ToString("F0") + "%";

            // Update unity / completes the frame
            await Awaitable.NextFrameAsync();

            // Activate the scene when fully loaded
            if (operation.progress >= 0.9f)
                break;
        }

        // To allow the loader to show
        await Awaitable.WaitForSecondsAsync(0.25f);

        operation.allowSceneActivation = true;
    }

    private void FixedUpdate()
    {
        float newRotation = SpinningThing.rotation.z + rotationSpeed;
        SpinningThing.Rotate(0, 0, newRotation);
    }
}
