using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ZSpawning2 : MonoBehaviour
{
    [SerializeField] private AudioSource roundStart;
    [SerializeField] float minRespawnWaveTime = 1f;
    [SerializeField] float maxRespawnWaveTime = 5f;
    [SerializeField] ushort minSpawnsPerWave = 1, maxSpawnsPerWave = 2;
    [SerializeField] float spawnPadding = 5f;
    [SerializeField] Transform[] spawnTransforms;
    [SerializeField] EnemySpawnType[] enemies;

    private List<ZSpawnPoint> spawns;
    public UnityEvent OnEnemyKilled, OnEnemySpawn, OnRoundStart;

    public float roundBufferTime = 5;
    public int enemiesInRound = 6;
    public int enemiesAlive = 0;
    public int maxEnemiesSpawnedIn = 24;
    public int enemiesKilled = 0;
    public int round = 0;
    public float spawnEnableDistance = 25f;
    public float spawnDisableDistance = 10f;
    public float healthIncreaseScale = 1.1f;

    private Transform player;
    
    private void Start()
    {
        player = GameObject.Find("Player").transform;
        
        spawns = new List<ZSpawnPoint>();

        // Initialize Spawns
        foreach (var spawnTransform in spawnTransforms)
        {
            spawns.Add(new ZSpawnPoint() {position = spawnTransform.position});
        }
        
        StartCoroutine(SpawnLoop());

        Enemy.globalEnemyHealthScale = 0.5f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            RoundComplete();
            OnRoundStart?.Invoke();
        }
        
        if (Input.GetKeyDown(KeyCode.F11))
        {
            LevelStats.main.GiveScore(1000);
        }
        
        if (Input.GetKeyDown(KeyCode.F10))
        {
            PlayerStatus.player.health += 1000;
        }
    }

    IEnumerator SpawnLoop()
    {
        Enemy.globalEnemyHealthScale = 1;
        
        while (true)
        {
            yield return new WaitForSeconds(roundBufferTime);
            OnRoundStart?.Invoke();
            
            DetermineActiveSpawns();
            int enemiesSpawned = 0;
            while (enemiesSpawned < enemiesInRound)
            {
                if (enemiesAlive >= maxEnemiesSpawnedIn)
                {
                    yield return new WaitForSeconds(Random.Range(minRespawnWaveTime, maxRespawnWaveTime));
                    continue;
                }
                
                int amountToSpawn = Random.Range(minSpawnsPerWave, maxSpawnsPerWave);

                for (int o = 0; o < amountToSpawn; o++)
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
            while (enemiesAlive > 0)
            {
                yield return new WaitForSeconds(3);
            }
            
            // Scale Rounds
            RoundComplete();
            roundStart.Play();
        }
    }

    public void AddSpawn(Vector3 spawnLocation)
    {
        spawns.Add(new ZSpawnPoint() {position = spawnLocation});
    }

    void DetermineActiveSpawns()
    {
        for (int i = 0; i < spawns.Count; i++)
        {
            if (Vector3.Distance(spawns[i].position, player.position) > spawnEnableDistance && Vector3.Distance(spawns[i].position, player.position) < spawnDisableDistance)
            {
                spawns[i].active = false;
            }
            else
            {
                spawns[i].active = true;
            }
        }
    }
    
    void RoundComplete()
    {
        // Every 3 rounds
        if (round % 3 == 0)
        {
            // Scale enemy health
            Enemy.globalEnemyHealthScale *= healthIncreaseScale;
        }
        
        // Unlock types
        switch(round)
        {
            case 3: // Unlock quiz
                enemies[1].spawnWeight = 1;
                break;
            
            case 5: // make spawns more dense
                maxSpawnsPerWave = 2;
                break;

            case 8: // unlock fudgee bar
                enemies[2].spawnWeight = 1;
                healthIncreaseScale = 1.2f;
                break;
            
            case 10: // unlock healer
                enemies[3].spawnWeight = 1;
                break;
            
            case 13: // increase max count
                maxEnemiesSpawnedIn = 18;
                maxSpawnsPerWave = 3;
                break;
            
            case 18: // make spawns faster
                minRespawnWaveTime = 1f;
                maxRespawnWaveTime = 1.5f;
                break;
        }
        
        // Scale enemy amounts
        enemiesInRound = Mathf.RoundToInt(enemiesInRound * 1.2f);

        round++;
    }
    
    void SpawnEnemy()
    {
        // Choose enemy
        Enemy enemy = ChooseEnemy();
                
        // Choose Spawnpoint
        
        List<ZSpawnPoint> activeSpawns = new List<ZSpawnPoint>();
        foreach (var spawn in spawns)
        {
            if (spawn.active) activeSpawns.Add(spawn);
        }
        
        Vector3 point = activeSpawns[Random.Range(0, spawns.Count)].position;
        // Spawn enemy
                
        var _enemy = Instantiate(enemy, position: point, 
            Quaternion.identity, transform);

        _enemy.onEnemyDeath.AddListener(enemyDeath);
        OnEnemySpawn?.Invoke();
        
        enemiesAlive++; 
    }
    
    Enemy ChooseEnemy()
    {
        Enemy enemyChosen;

        List<Enemy> list = new List<Enemy>();
        for (int i = 0; i < enemies.Length; i++)
        {
            for (int o = 0; o < enemies[i].spawnWeight; o++)
            {
                list.Add(enemies[i].prefab);
            }
        }

        enemyChosen = list[Random.Range(0, list.Count)];

        return enemyChosen;
    }

    void enemyDeath(EnemyType _)
    {
        enemiesKilled++;
        enemiesAlive--;
        OnEnemyKilled?.Invoke();
    }
    
    [System.Serializable]
    public class ZSpawnPoint
    {
        public Vector3 position;
        public bool active;
    }
}

[System.Serializable]
public struct EnemySpawnType
{
    [Range(0, 100)]
    public int spawnWeight;
    
    [FormerlySerializedAs("enemy")] public Enemy prefab;
}