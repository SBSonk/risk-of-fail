using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossEnemySpawner : BossAttackV2
{
    public Transform[] spawnPoints;
    public SpawnWeight[] enemies;

    public int minEnemiesPerWave, maxEnemiesPerWave;
    public float timeBetweenSpawns = .25f;
    public int globalMaxEnemies = 8;
    private int globalEnemies;

    public override IEnumerator Attack()
    {
        int enemyCount = Random.Range(minEnemiesPerWave, maxEnemiesPerWave);

        for (int i = 0; i < enemyCount; i++)
        {
            if (globalEnemies > globalMaxEnemies) break;
            
            Alive enemyToSpawn = ChooseEnemy();
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            var newEnemy = Instantiate(enemyToSpawn, spawnPoint.position, quaternion.identity);
            globalEnemies++;
            newEnemy.onDeath.AddListener((_) =>
            {
                globalEnemies--;
            });

            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
    
    Alive ChooseEnemy()
    {
        List<Alive> bag = new List<Alive>();

        foreach (var e in enemies)
        {
            for (int i = 0; i < e.spawnWeight; i++)
            {
                bag.Add(e.enemyPrefab);
            }
        }

        return bag[Random.Range(0, bag.Count)];
    }
    
    [Serializable]
    public struct SpawnWeight
    {
        public Alive enemyPrefab;
        public int spawnWeight;
    }
}
