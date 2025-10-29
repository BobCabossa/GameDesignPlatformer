using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private enum States { Cooldown, Spawning }

    public Transform spawnDonePoint;

    [Space(10)]
    public GameObject enemyPrefab;
    public float CooldownTime = 10;
    public float SpawningSpeed = 10;
    public bool SpawningToTheLeft = true;

    [Header("Debug")]
    private float realCooldown = 0;
    private States state = States.Cooldown;
    private Enemy EnemySpawning;

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
        Transform transform = EnemySpawning.transform;
        transform.position = Vector2.MoveTowards(
            transform.position,
            spawnDonePoint.position,
            SpawningSpeed);

        if (Vector2.Distance(transform.position, spawnDonePoint.position) < 0.001f)
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
        GameObject newEnemy = Instantiate(enemyPrefab);
        
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
}
