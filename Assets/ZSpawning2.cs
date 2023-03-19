using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.Serialization;

public class ZSpawning2 : MonoBehaviour
{
    [SerializeField] float minRespawnWaveTime = 1f;
    [SerializeField] float maxRespawnWaveTime = 5f;
    [SerializeField] ushort minSpawnsPerWave = 1, maxSpawnsPerWave = 2;
    [SerializeField] float spawnPadding = 5f;
    [SerializeField] Transform[] spawnTransforms;
    [SerializeField] EnemySpawnType[] enemies;
    
    public UnityEvent OnEnemyKilled, OnEnemySpawn, OnRoundStart;

    public float roundBufferTime = 5;
    public int enemiesInRound = 6;
    public int enemiesAlive = 0;
    public int maxEnemiesSpawnedIn = 24;
    public int enemiesKilled = 0;
    public int round = 0;
    
    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        Enemy.globalEnemyHealthScale = 1;
        
        while (true)
        {
            yield return new WaitForSeconds(roundBufferTime);
            OnRoundStart?.Invoke();
            
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
                    SpawnEnemy();
                
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
            
            round++;
            // Scale Rounds
            RoundComplete();
        }
    }

    void RoundComplete()
    {
        // Every 3 rounds
        if (round % 3 == 0)
        {
            // Scale enemy health
            Enemy.globalEnemyHealthScale *= 1.1f;
        }
        
        // Unlock types
        switch(round)
        {
            case 3: // Unlock quiz
                enemies[1].spawnWeight = 2;
                break;
            
            case 5: // make spawns more dense
                maxSpawnsPerWave = 3;
                break;

            case 8: // unlock fudgee bar
                enemies[2].spawnWeight = 1;
                break;
            
            case 10: // unlock healer
                enemies[3].spawnWeight = 1;
                break;
            
            case 13: // increase max count
                maxEnemiesSpawnedIn = 18;
                break;
            
            case 15: // make spawns faster
                minRespawnWaveTime = .75f;
                maxRespawnWaveTime = 1.5f;
                break;
        }
        
        // Scale enemy amounts
        enemiesInRound = Mathf.RoundToInt(enemiesInRound * 1.2f);
    }
    
    void SpawnEnemy()
    {
        // Choose enemy
        Enemy enemy = ChooseEnemy();
                
        // Choose Spawnpoint
        Vector3 point = spawnTransforms[Random.Range(0, spawnTransforms.Length)].position;
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
}

[System.Serializable]
public struct EnemySpawnType
{
    [Range(0, 100)]
    public int spawnWeight;
    
    [FormerlySerializedAs("enemy")] public Enemy prefab;
}