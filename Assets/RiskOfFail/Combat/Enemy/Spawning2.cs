using System;
using System.Collections.Generic;
using System.Linq;
using RiskOfFail.Combat;
using RiskOfFail.Combat.Enums;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Spawning2 : MonoBehaviour
{
    [SerializeField] private float minRespawnWaveTime = 1f, respawnWaveTime = 5f;
    [SerializeField] private ushort minSpawnsPerWave = 1, maxSpawnsPerWave = 2;
    [SerializeField] private float spawnPadding = 5f;
    [SerializeField] private Transform[] spawnTransforms;
    [SerializeField] private EnemySpawn[] enemies;
    [SerializeField] private bool spawnOnStart;
    public UnityEvent OnEnemyKilled, OnEnemySpawn;
    private List<SpawnPoint> enabledSpawns;

    private int enemiesKilled;

    private List<SpawnPoint> spawnPoints;

    private void Start()
    {
        // Initialize spawnpoints
        spawnPoints = new List<SpawnPoint>();
        for (var i = 0; i < spawnTransforms.Length; i++) spawnPoints.Add(new SpawnPoint(spawnTransforms[i]));
    }

    private void Update()
    {
        if (!PlayerStatus.instance) return;

        UpdateSpawnerDistances();
    }

    private void UpdateSpawnerDistances()
    {
        for (var i = 0; i < spawnPoints.Count; i++)
            spawnPoints[i].DistanceToPlayer =
                Vector3.Distance(spawnPoints[i].position, PlayerStatus.instance.transform.position);
    }

    private void SetValidSpawns()
    {
        enabledSpawns = new List<SpawnPoint>();

        for (var i = 0; i < spawnPoints.Count; i++)
            if (spawnPoints[i].DistanceToPlayer >= spawnPadding)
                enabledSpawns.Add(spawnPoints[i]);
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
        for (var i = 0; i < amount; i++) TrySpawn();

        CancelInvoke();
    }

    private void TrySpawn()
    {
        // Decide how much enemies to spawn
        var possibleNumbers = Enumerable.Range(0, enemies.Length).ToList();

        spawnPoints.Sort((x, y) => x.DistanceToPlayer.CompareTo(y.DistanceToPlayer));
        SetValidSpawns();

        for (var i = 0; i < Random.Range(minSpawnsPerWave, maxSpawnsPerWave + 1); i++)
        {
            // Choose enemy to spawn
            var chosen = Random.Range(0, possibleNumbers.Count);

            // Skip if over spawn limit or failed spawn rng
            if (enemies[chosen].alive >= enemies[chosen].maxAlive || enemies[chosen].spawnChance / 100f < Random.value)
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

    private void SpawnEnemy(EnemySpawn enemy)
    {
        // Choose where to spawn
        var point = Random.Range(0, enabledSpawns.Count);
        var _enemy = Instantiate(enemy.prefab, enabledSpawns[point].position,
            Quaternion.identity, transform).GetComponent<Enemy>();

        _enemy.onDeath.AddListener(enemyDeath);
        OnEnemySpawn?.Invoke();
    }

    // Reduce enemy count when an enemy dies
    private void enemyDeath(DamageTypeFlag type)
    {
        /*// Get types
        for (var i = 0; i < enemies.Length; i++)
            if (enemies[i].type == type)
            {
                enemies[i].alive--;
                enemiesKilled++;
                OnEnemyKilled?.Invoke();
                return;
            }*/
    }
}

[Serializable]
public struct EnemySpawn
{
    public EnemyType type;
    public GameObject prefab;
    [Range(0, 100)] public int spawnChance; // Spawn chance percentage
    public ushort alive, maxAlive;
}

[Serializable]
public enum EnemyType
{
    WrittenWorks,
    Quiz,
    Notebook,
    TripleQuiz,
    QuarterlyAssessment,
    Healer,
    Boss
}

[Serializable]
public class SpawnPoint
{
    public Vector3 position;
    public float DistanceToPlayer;

    public SpawnPoint(Transform t)
    {
        position = t.position;
    }
}