using System;
using System.Collections;
using System.Collections.Generic;
using RiskOfFail.Combat;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ZSpawning2 : MonoBehaviour
{
    [SerializeField] private AudioSource roundStart;
    [SerializeField] private float minRespawnWaveTime = 1f;
    [SerializeField] private float maxRespawnWaveTime = 5f;
    [SerializeField] private ushort minSpawnsPerWave = 1, maxSpawnsPerWave = 2;
    [SerializeField] private float spawnPadding = 5f;
    [SerializeField] private Transform[] spawnTransforms;
    [SerializeField] private EnemySpawnType[] enemies;
    public UnityEvent OnEnemyKilled, OnEnemySpawn, OnRoundStart;

    public float roundBufferTime = 5;
    public int enemiesInRound = 6;
    public int enemiesAlive;
    public int maxEnemiesSpawnedIn = 24;
    public int enemiesKilled;
    public int round;
    public float spawnEnableDistance = 25f;
    public float spawnDisableDistance = 10f;
    public float healthIncreaseScale = 1.1f;
    public float maxHealthScale = 4;
    public int maxEnemiesInRound = 255;

    private Transform player;

    private List<ZSpawnPoint> spawns;

    private void Start()
    {
        player = GameObject.Find("Player").transform;

        spawns = new List<ZSpawnPoint>();

        // Initialize Spawns
        foreach (var spawnTransform in spawnTransforms)
            spawns.Add(new ZSpawnPoint { position = spawnTransform.position });

        StartCoroutine(SpawnLoop());

        Enemy.globalEnemyHealthScale = 0.5f;
    }

    private void Update()
    {
#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.F12))
        {
            RoundComplete();
            OnRoundStart?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.F11)) LevelStats.main.GiveScore(1000);

        if (Input.GetKeyDown(KeyCode.F10)) PlayerStatus.player.health += 1000;

#endif
    }

    private IEnumerator SpawnLoop()
    {
        Enemy.globalEnemyHealthScale = 1;

        while (true)
        {
            yield return new WaitForSeconds(roundBufferTime);
            OnRoundStart?.Invoke();

            DetermineActiveSpawns();
            var enemiesSpawned = 0;
            while (enemiesSpawned < enemiesInRound)
            {
                if (enemiesAlive >= maxEnemiesSpawnedIn)
                {
                    yield return new WaitForSeconds(Random.Range(minRespawnWaveTime, maxRespawnWaveTime));
                    continue;
                }

                var amountToSpawn = Random.Range(minSpawnsPerWave, maxSpawnsPerWave);

                for (var o = 0; o < amountToSpawn; o++)
                {
                    try
                    {
                        SpawnEnemy();
                    }
                    catch
                    {
                        continue;
                    }


                    enemiesSpawned++;
                }

                yield return new WaitForSeconds(Random.Range(minRespawnWaveTime, maxRespawnWaveTime));
            }
            // Wait for all enemies to die

            // wait for next round
            while (enemiesAlive > 0) yield return new WaitForSeconds(3);

            // Scale Rounds
            RoundComplete();
            roundStart.Play();
        }
    }

    public void AddSpawn(Vector3 spawnLocation)
    {
        spawns.Add(new ZSpawnPoint { position = spawnLocation });
    }

    private void DetermineActiveSpawns()
    {
        for (var i = 0; i < spawns.Count; i++)
            if (Vector3.Distance(spawns[i].position, player.position) > spawnEnableDistance &&
                Vector3.Distance(spawns[i].position, player.position) < spawnDisableDistance)
                spawns[i].active = false;
            else
                spawns[i].active = true;
    }

    private void RoundComplete()
    {
        try
        {
            DiscordRPCManager.instance.ChangeDiscordState("Horde Mode - Round " + round, "Solo");
        }
        catch
        {
            Debug.LogWarning("Discord not connected.");
        }

        // Every 3 rounds
        if (round % 3 == 0)
        {
            // Scale enemy health
            Enemy.globalEnemyHealthScale *= healthIncreaseScale;

            Enemy.globalEnemyHealthScale = Enemy.globalEnemyHealthScale > maxHealthScale
                ? maxHealthScale
                : Enemy.globalEnemyHealthScale;
        }

        // Unlock types
        switch (round)
        {
            case 3: // Unlock quiz
                enemies[1].spawnWeight = 1;
                break;

            case 5: // make spawns more dense
                maxSpawnsPerWave = 2;
                break;

            case 8: // unlock fudgee bar
                enemies[2].spawnWeight = 1;
                healthIncreaseScale += .13f;
                break;

            // unlock dogs
            case 10:
                enemies[4].spawnWeight = 2;
                break;

            case 12: // unlock triple quiz
                enemies[5].spawnWeight = 1;
                break;


            case 16: // unlock healer
                enemies[3].spawnWeight = 1;
                break;

            case 13: // increase max count
                maxEnemiesSpawnedIn = 18;
                minSpawnsPerWave = 2;
                maxSpawnsPerWave = 3;
                break;

            case 18: // make spawns faster
                minRespawnWaveTime = 1f;
                maxRespawnWaveTime = 1.5f;
                break;

            case 20:
                maxEnemiesSpawnedIn = 20;
                break;

            case 24:
                maxEnemiesSpawnedIn = 22;
                break;

            case 28:
                maxEnemiesSpawnedIn = 24;
                break;
        }

        // Scale enemy amounts
        enemiesInRound = enemiesInRound < maxEnemiesInRound
            ? Mathf.RoundToInt(enemiesInRound * 1.1f)
            : maxEnemiesInRound;

        round++;
    }

    private void SpawnEnemy()
    {
        // Choose enemy
        var enemy = ChooseEnemy();

        // Choose Spawnpoint

        var activeSpawns = new List<ZSpawnPoint>();
        foreach (var spawn in spawns)
            if (spawn.active)
                activeSpawns.Add(spawn);

        var point = activeSpawns[Random.Range(0, spawns.Count)].position;
        // Spawn enemy

        var _enemy = Instantiate(enemy, point,
            Quaternion.identity, transform);

        _enemy.onEnemyDeath.AddListener(enemyDeath);
        OnEnemySpawn?.Invoke();

        enemiesAlive++;
    }

    private Enemy ChooseEnemy()
    {
        var list = new List<Enemy>();
        for (var i = 0; i < enemies.Length; i++)
        for (var o = 0; o < enemies[i].spawnWeight; o++)
            list.Add(enemies[i].prefab);

        var enemyChosen = list[Random.Range(0, list.Count)];

        return enemyChosen;
    }

    private void enemyDeath(EnemyType _)
    {
        enemiesKilled++;
        enemiesAlive--;
        OnEnemyKilled?.Invoke();
    }

    [Serializable]
    public class ZSpawnPoint
    {
        public Vector3 position;
        public bool active;
    }
}

[Serializable]
public struct EnemySpawnType
{
    [Range(0, 100)] public int spawnWeight;

    [FormerlySerializedAs("enemy")] public Enemy prefab;
}