using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawning2 : MonoBehaviour
{
    [SerializeField] float minRespawnWaveTime = 1f, respawnWaveTime = 5f;
    [SerializeField] ushort minSpawnsPerWave = 1, maxSpawnsPerWave = 2;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] enemySpawn[] enemies;

    int enemiesKilled = 0;
    public UnityEvent OnEnemyKilled, OnEnemySpawn;

    // Type Counts
    public static Spawning2 active;

    void Start()
    {
        if (!active) active = this;

        // Repeating spawns
        Invoke("TrySpawn", respawnWaveTime);
    }

    void TrySpawn()
    {
        // Decide how much enemies to spawn
        List<int> possibleNumbers = Enumerable.Range(0, enemies.Length).ToList();
        int chosen;

        for (int i = 0; i < Random.Range(minSpawnsPerWave, maxSpawnsPerWave); i++)
        {
            // Choose enemy to spawn
            chosen = Random.Range(0, possibleNumbers.Count);

            // Skip if over spawn limit or failed spawn rng
            if (enemies[chosen].alive >= enemies[chosen].maxAlive || (enemies[chosen].spawnChance / 100f) < Random.value)
            {
                possibleNumbers.Remove(chosen);
                continue;
            }

            // Try to spawn enemy
            SpawnEnemy(enemies[chosen]); 
            enemies[chosen].alive++;
        }

        // Invoke next spawnWave
        Invoke("TrySpawn", Random.Range(minRespawnWaveTime, respawnWaveTime));
    }

    // Try's to spawn enemy, returns false otherwise
    void SpawnEnemy(enemySpawn enemy)
    {
        // Choose where to spawn
        int point = Random.Range(0, spawnPoints.Length);
        var _enemy = Instantiate(enemy.prefab, position: spawnPoints[point].position, Quaternion.identity, transform).GetComponent<Enemy>();

        _enemy.onEnemyDeath.AddListener(enemyDeath);
    }

    // Reduce enemy count when an enemy dies
    void enemyDeath(EnemyType type)
    {
        // Get types
        for (int i = 0; i < active.enemies.Length; i++)
        {
            if (active.enemies[i].type == type)
            {
                active.enemies[i].alive--;
                active.enemiesKilled++;
                active.OnEnemyKilled?.Invoke();
                return;
            }
        }
    }

    [System.Serializable]
    public struct enemySpawn
    {
        public EnemyType type;
        public GameObject prefab;
        [Range(0, 100)]
        public int spawnChance; // Spawn chance percentage
        public ushort alive, maxAlive;
    }
}

[System.Serializable]
public enum EnemyType
{
    WrittenWorks,
    Quiz,
    QuarterlyAssessment,
    Boss
}