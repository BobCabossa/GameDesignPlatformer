using UnityEngine;

public class Collectable : MonoBehaviour
{
    [Header("Movement")]
    public float distance = 0.35f;
    public float speed = 0.01f;

    [Space(5), Tooltip("Debug value")]
    public SceneNames SceneName;
    public Transform body;

    private bool up = false;

    private void Start()
    {
        SceneName = SceneLoader.GetSceneName();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ColliderHelper.IsIt<Player>(collision))
        {
            Destroy(gameObject);
            SaveManager.SaveCollectable(SceneName);
        }
    }

    private void FixedUpdate()
    {
        Vector2 target = new(0, up ? distance : -distance);
        body.localPosition = Vector2.MoveTowards(body.localPosition, target, speed);
        if (Vector2.Distance(body.localPosition, target) <= 0.001f)
        {
            up = !up;
        }
    }
}
