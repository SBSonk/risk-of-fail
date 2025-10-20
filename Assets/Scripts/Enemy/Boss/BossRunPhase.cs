using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Dreamteck.Splines;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BossRunPhase : MonoBehaviour
{
    public SplineFollower follower;
    public BossRoomFirstPhase firstPhase;

    public CinemachineVirtualCamera vcam;
    public Collider2D phaseCameraBounds, lastPhaseBounds;
    
    public Enemy[] enemyPrefabs;
    public int minEnemiesPerWave = 2, maxEnemiesPerWave = 4;
    public float timeBetweenEnemies = .25f;
    public float minTimeBetweenWaves = 2.5f, maxTimeBetweenWaves = 5f;

    public float normalSpeed = 12;
    [FormerlySerializedAs("spawnSpeed")] public float fastSpeed = 6f;
    public float slowTime = 6f;

    public UnityEvent OnPhaseStart;
    
    private float targetSpeed;

    private int index = 0;

    public UnityEvent OnReachFirstWall, OnReachSecondWall;

    private void Start()
    {
        follower.enabled = true;
        follower.onEndReached += ReachedEnd;
        targetSpeed = normalSpeed;

        vcam.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = phaseCameraBounds;
        vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = new Vector3(0, 2);
    }

    private void OnEnable()
    {
        follower.followSpeed = normalSpeed;
        firstPhase.enabled = false;
        StartCoroutine(SpawnLoop());
        
        OnPhaseStart?.Invoke();
    }

    public void ResetSplinePosition() => follower.SetPercent(0);
    
    private void OnDestroy()
    {
        StopLoop();
    }

    private void OnDisable()
    {
        StopLoop();
    }

    private void FixedUpdate()
    {
        follower.followSpeed = Mathf.Lerp(follower.followSpeed, targetSpeed, .25f);
    }

    void ReachedEnd(double _)
    {
        if (index == 0) OnReachFirstWall?.Invoke();
        if (index == 1)
        {
            OnReachSecondWall?.Invoke();
            vcam.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = lastPhaseBounds;
            vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = new Vector3(0, 2);
        }
        index++;
        
        StopLoop();
    }
    
    void StopLoop()
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
        }
    }

    public void SpeedUp() => targetSpeed = fastSpeed;

    public void SlowDown() => targetSpeed = normalSpeed;
}
