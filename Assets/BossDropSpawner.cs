using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossDropSpawner : MonoBehaviour
{
    [SerializeField] Rigidbody2D ammoPrefab, healthPrefab;
    [SerializeField] private int minSpawns, maxSpawns;
    [SerializeField] private float minImpulse, maxImpulse;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, minImpulse);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxImpulse);
    }

    public void SpawnItems()
    {
        for (int i = 0; i < Random.Range(minSpawns, maxSpawns); i++)
        {
            Rigidbody2D spawnedObject;
            
            if (Random.Range(0, 4) != 0)
            {
                spawnedObject = Instantiate(ammoPrefab, transform.position, quaternion.identity);
            }
            else
            {
                spawnedObject = Instantiate(healthPrefab, transform.position, quaternion.identity);
            }
            
            Vector3 force = Random.insideUnitCircle * Random.Range(minImpulse, maxImpulse);
            spawnedObject.AddForce(force, ForceMode2D.Impulse);
        }
    }
}
