using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossRunPhase : MonoBehaviour
{
    public SplineFollower follower;
    public BossRoomFirstPhase firstPhase;
    
    public Enemy[] enemyPrefabs;
    public int minEnemiesPerWave = 2, maxEnemiesPerWave = 4;
    public float timeBetweenEnemies = .25f;
    public float minTimeBetweenWaves = 2.5f, maxTimeBetweenWaves = 5f;

    public float normalSpeed = 12, spawnSpeed = 6f;
    public float slowTime = 6f;

    private float targetSpeed;
    
    private void Start()
    {
        follower.onEndReached += StopLoop;
        targetSpeed = normalSpeed;
    }

    private void OnEnable()
    {
        follower.followSpeed = normalSpeed;
        firstPhase.enabled = false;
        StartCoroutine(SpawnLoop());
    }

    private void OnDestroy()
    {
        StopLoop(0);
    }

    private void OnDisable()
    {
        StopLoop(0);
    }

    private void FixedUpdate()
    {
        follower.followSpeed = Mathf.Lerp(follower.followSpeed, targetSpeed, .25f);
    }

    void StopLoop(Double _)
    {
        StopAllCoroutines();
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTimeBetweenWaves, maxTimeBetweenWaves));
            
            int amountToSpawn = Random.Range(minEnemiesPerWave, maxEnemiesPerWave);
            
            for (int i = 0; i < amountToSpawn; i++)
            {
                Enemy enemyToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                Instantiate(enemyToSpawn, transform.position, quaternion.identity);
            
                yield return new WaitForSeconds(timeBetweenEnemies);
            }
        
            SlowDown();
            yield return new WaitForSeconds(slowTime);
            SpeedUp();
        }
    }

    void SpeedUp() => targetSpeed = normalSpeed;

    void SlowDown() => targetSpeed = spawnSpeed;
}
