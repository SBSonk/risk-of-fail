using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using Random = UnityEngine.Random;

public class Spawning2 : MonoBehaviour
{
    [SerializeField] float minRespawnWaveTime = 1f, respawnWaveTime = 5f;
    [SerializeField] ushort minSpawnsPerWave = 1, maxSpawnsPerWave = 2;
    [SerializeField] float spawnPadding = 5f;
    [SerializeField] Transform[] spawnTransforms;
    [SerializeField] enemySpawn[] enemies;
    [SerializeField] bool spawnOnStart;

    int enemiesKilled = 0;
    public UnityEvent OnEnemyKilled, OnEnemySpawn;

    List<SpawnPoint> spawnPoints;
    List<SpawnPoint> enabledSpawns;

    private void Start()
    {
        // Initialize spawnpoints
        spawnPoints = new List<SpawnPoint>();
        for (int i = 0; i < spawnTransforms.Length; i++)
        {
            spawnPoints.Add(new SpawnPoint(spawnTransforms[i]));
        }
    }

    private void Update()
    {
        if (!PlayerStatus.IsAlive) return;

        UpdateSpawnerDistances();
    }

    void UpdateSpawnerDistances()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            spawnPoints[i].DistanceToPlayer = Vector3.Distance(spawnPoints[i].position, PlayerStatus.player.transform.position);
        }
    }

    void SetValidSpawns()
    {
        enabledSpawns = new List<SpawnPoint>();

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if (spawnPoints[i].DistanceToPlayer >= spawnPadding) enabledSpawns.Add(spawnPoints[i]);
        }
    }

    public void StartSpawner()
    {
        // Repeating spawns
        if (spawnOnStart) TrySpawn();
        else Invoke("TrySpawn", respawnWaveTime);
    }

    public void StopSpawner()
    {
        CancelInvoke();
    }

    public void TrySpawn(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            TrySpawn();
        }
        
        CancelInvoke();
    }
    
    void TrySpawn()
    {
        // Decide how much enemies to spawn
        List<int> possibleNumbers = Enumerable.Range(0, enemies.Length).ToList();

        spawnPoints.Sort((x, y) => x.DistanceToPlayer.CompareTo(y.DistanceToPlayer));
        SetValidSpawns();

        for (int i = 0; i < Random.Range(minSpawnsPerWave, maxSpawnsPerWave + 1); i++)
        {
            // Choose enemy to spawn
            int chosen = Random.Range(0, possibleNumbers.Count);

            // Skip if over spawn limit or failed spawn rng
            if (enemies[chosen].alive >= enemies[chosen].maxAlive || (enemies[chosen].spawnChance / 100f) < Random.value)
            {
                possibleNumbers.Remove(chosen);
                continue;
            }

            SpawnEnemy(enemies[chosen]); 
            enemies[chosen].alive++;
        }

        // Invoke next spawnWave
        Invoke("TrySpawn", Random.Range(minRespawnWaveTime, respawnWaveTime));
    }

    void SpawnEnemy(enemySpawn enemy)
    {
        // Choose where to spawn
        int point = Random.Range(0, enabledSpawns.Count);
        var _enemy = Instantiate(enemy.prefab, position: enabledSpawns[point].position, 
            Quaternion.identity, transform).GetComponent<Enemy>();

        _enemy.onEnemyDeath.AddListener(enemyDeath);
        OnEnemySpawn?.Invoke();
        
    }

    // Reduce enemy count when an enemy dies
    void enemyDeath(EnemyType type)
    {
        // Get types
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].type == type)
            {
                enemies[i].alive--;
                enemiesKilled++;
                OnEnemyKilled?.Invoke();
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

[System.Serializable]
public class SpawnPoint
{
    public Vector3 position;
    public float DistanceToPlayer;

    public SpawnPoint(Transform t)
    {
        position = t.position;
    }
}