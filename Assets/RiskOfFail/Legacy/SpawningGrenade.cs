using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningGrenade : Throwable
{
    [SerializeField] private GameObject[] objectsToSpawn;

    protected override void Explode()
    {
        SpawnRandomObject();
    }

    private void SpawnRandomObject()
    {
        if (objectsToSpawn.Length == 0)
            return;

        int randomIndex = Random.Range(0, objectsToSpawn.Length);
        GameObject objectToSpawn = objectsToSpawn[randomIndex];

        // Spawn the object at the grenade's position with a random rotation
        Instantiate(objectToSpawn, transform.position, Quaternion.identity);
    }
}
