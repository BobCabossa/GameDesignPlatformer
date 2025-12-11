using UnityEngine;

public class Collectable : MonoBehaviour
{
    [Header("Distance from ground")]
    public bool useMaxDistanceFromGround = true;
    public float distanceFromGround = 1.0f;

    [Header("Movement")]
    public float distance = 0.35f;
    public float speed = 0.01f;

    [Space(5), Tooltip("Debug value")]
    public SceneNames SceneName;
    public Transform body;

    private bool up = false;

    private void Awake()
    {
        if (!useMaxDistanceFromGround)
            return;

        int wallLayerMask = LayerMask.GetMask("Ground");
        Vector2 direction = Vector2.down;
        Vector2 origin = (Vector2)transform.position;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, int.MaxValue, wallLayerMask);

        if (hit.collider == null)
        {
            Debug.LogWarning($"Collectable; {name}, no ground below collectable.");
            return;
        }

        Vector2 colliderPosistion = hit.point;
        colliderPosistion.y += distanceFromGround;

        transform.position = colliderPosistion;
    }

    private void Start()
    {
        if (SceneName == SceneNames.None)
        {
            SceneName = SceneLoader.GetSceneName();
        }
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = GizmosSettings.Color;
        Vector2 direction = Vector2.down;
        Vector2 origin = (Vector2)transform.position;

        Gizmos.DrawLine(origin, origin + direction * distanceFromGround);
    }
}
