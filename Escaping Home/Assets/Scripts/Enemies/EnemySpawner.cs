using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private enum States { Cooldown, Spawning }

    public Vector2 spawnDonePoint;

    [Space(10)]
    public GameObject enemyPrefab;
    public float CooldownTime = 10;
    public float SpawningSpeed = 10;
    public bool SpawningToTheLeft = true;

    [Header("Debug")]
    private float realCooldown = 0;
    private States state = States.Cooldown;
    private Enemy EnemySpawning;
    private Transform enemyRoot;

    private Vector2 GetSpawnPoint() => (Vector2)transform.position + spawnDonePoint;

    private void Awake()
    {
        enemyRoot = transform.root;
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case States.Cooldown:
                Cooldown();
                break;
            case States.Spawning:
                Spawning();
                break;
            default:
                break;
        }
    }

    private void Spawning()
    {
        if (EnemySpawning == null) return;

        Vector2 spawnPoint = GetSpawnPoint();
        Transform transform = EnemySpawning.transform;
        transform.position = Vector2.MoveTowards(
            transform.position,
            spawnPoint,
            SpawningSpeed);

        if (Vector2.Distance(transform.position, spawnPoint) < 0.001f)
        {
            state = States.Cooldown;
            EnemySpawning.Collider.enabled = true;
            EnemySpawning.enabled = true;
            EnemySpawning = null;
        }
    }

    private void Cooldown()
    {
        if (realCooldown > 0)
        {
            realCooldown -= Time.deltaTime;
            return;
        }

        realCooldown = CooldownTime;
        SpawnEnemy();
        state = States.Spawning;
    }

    private void SpawnEnemy()
    {
        // Spawns the enemy
        GameObject newEnemy = Instantiate(enemyPrefab, enemyRoot);

        // Making the spawned enemy ready to be plads inside spawner
        EnemySpawning = newEnemy.GetComponent<Enemy>();
        if (SpawningToTheLeft)
        {
            EnemySpawning.TurnAround();
        }
        EnemySpawning.Collider.enabled = false;
        EnemySpawning.enabled = false;


        // Moves the enemy
        newEnemy.transform.position = transform.position;
    }

    private void OnDrawGizmos()
    {
        if (spawnDonePoint != null)
        {
            Gizmos.color = GizmosSettings.Color;
            Gizmos.DrawSphere(GetSpawnPoint(), GizmosSettings.Radius);
        }
    }
}
