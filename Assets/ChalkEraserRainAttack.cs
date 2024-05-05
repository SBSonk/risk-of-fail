using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChalkEraserRainAttack : BossAttackV2
{
    public Collider2D[] spawnArea;
    public GameObject prefab;

    public float padding = 1;
    public int minSpawns = 2, maxSpawns = 4;
    public float timeBetweenSpawns = .25f;

    private void Start()
    {
        StartCoroutine(Attack());
    }

    public override IEnumerator Attack()
    {
        for (int i = 0; i < Random.Range(minSpawns, maxSpawns); i++)
        {
            var newEraser = Instantiate(prefab);
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
