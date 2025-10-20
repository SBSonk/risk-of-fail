using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossGlueAttack : BossAttack
{
    [SerializeField] GameObject gluePrefab;
    [SerializeField] private float checkRadius = 3;
    [SerializeField] private int minSoawns, maxSpawns;

    [SerializeField] private float spawnRadius;

    public override void UseAttack()
    {
        for (int i = 0; i < Random.Range(minSoawns, maxSpawns); i++)
        {
            Vector3 spawnPos = transform.position + (Vector3) Random.insideUnitCircle * Random.Range(1, spawnRadius);
        
            // Choose random spot
            Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPos, checkRadius);
            bool spotHasGlue = false;
        
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("BossGlueLayer") || collider.CompareTag("Boss"))
                {
                    spotHasGlue = true;
                    break;
                }
            }
        
            // Check if spot has glue
            if (!spotHasGlue)
            {
                // Spawn there if none
                Instantiate(gluePrefab, spawnPos, Quaternion.identity);
            }
        }
    }
}
