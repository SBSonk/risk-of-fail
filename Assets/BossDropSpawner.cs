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

    [SerializeField] private Vector2 direction = Vector2.left;

    public void SpawnItems()
    {
        print("spawned");
        
        for (int i = 0; i < Random.Range(minSpawns, maxSpawns); i++)
        {
            Vector3 direction = this.direction;
            direction.y *= Random.Range(-1, 1);
            
            direction.Normalize();
            
            // choose what to spawn
            // spawn more ammo than health

            Rigidbody2D spawnedObject;
            
            if (Random.value > .6f)
            {
                spawnedObject = Instantiate(ammoPrefab, transform.position, quaternion.identity);
            }
            else
            {
                spawnedObject = Instantiate(healthPrefab, transform.position, quaternion.identity);
            }
            
            spawnedObject.AddForce(direction * Random.Range(minImpulse, maxImpulse), ForceMode2D.Impulse);
        }
    }
}
