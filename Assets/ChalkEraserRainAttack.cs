using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChalkEraserRainAttack : BossAttackV2
{
    [Header("Attack")]
    public Collider2D[] spawnArea;
    public GameObject[] prefabs;
    public float padding = 1;
    public int minSpawns = 2, maxSpawns = 4;
    public float timeBetweenSpawns = .25f;
    public float hoverTime = 2f;
    
    [Header("Animation")]
    public GameObject staticPrefab;
    public float maxX = 15;
    public float minFloatTime = 2f;
    public float maxFloatTime = 4f;
    public float minTimeBetweenFloats = .25f;
    public float maxTimeBetweenFloats = .5f;

    private void Start()
    {
        StartCoroutine(Attack());
    }

    public override IEnumerator Attack()
    {
        int range = Random.Range(minSpawns, maxSpawns);
        // Animation
        for (int i = 0; i < range; i++)
        {
            var newStaticEraser = Instantiate(staticPrefab, transform.position + new Vector3(Random.Range(-maxX, maxX), -40),
                quaternion.identity);

            newStaticEraser.transform.DOMoveY(newStaticEraser.transform.position.y + 60, Random.Range(minFloatTime, maxFloatTime));
            yield return new WaitForSeconds(Random.Range(minTimeBetweenFloats, maxTimeBetweenFloats));
        }
        
        yield return new WaitForSeconds(hoverTime);
        
        // Spawn Attacks 
        for (int i = 0; i < range; i++)
        {
            var newEraser = Instantiate(prefabs[Random.Range(0, prefabs.Length)]);
            newEraser.transform.position = ChooseSpawnPoint();
            
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    public Vector3 ChooseSpawnPoint()
    {
        Collider2D col = spawnArea[Random.Range(0, spawnArea.Length)];

        Vector3 spawnPoint = new Vector3(Random.Range(col.bounds.min.x + padding, col.bounds.max.x - padding),
            Random.Range(col.bounds.min.y + padding, col.bounds.max.y - padding));

        return spawnPoint;
    }
}
