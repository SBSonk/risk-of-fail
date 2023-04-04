using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectSpawnRandomizer : MonoBehaviour
{
    [SerializeField] private RandomSpawnLocation[] potentialSpawns;
    [SerializeField] private int minAmount, maxAmount;

    private void Start()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        if (potentialSpawns.Length == 0)
        {
            Debug.LogError(gameObject.name + " No spawns set on spawnRandomizer.");
            return;
        }

        if (maxAmount > potentialSpawns.Length || minAmount > potentialSpawns.Length)
        {
            Debug.LogError(gameObject.name + " Min/Max spawns greater than potentialSpawns set on spawnRandomizer.");
            return;
        }

        // Spawn Prefabs
        for (int i = 0; i < Random.Range(minAmount, maxAmount + 1); i++)
        {
            // Choose location
            bool finishedChoosing = false;
            int chosenIndex = 0;
            
            while (!finishedChoosing)
            {
                chosenIndex = Random.Range(0, potentialSpawns.Length);
                if (!potentialSpawns[chosenIndex].spawned) finishedChoosing = true;
            }

            // Spawn object
            var spawn = potentialSpawns[chosenIndex];
            var prefab = spawn.randomizedPrefabs[0];
            if (spawn.randomizedPrefabs.Length > 1) prefab = spawn.randomizedPrefabs[Random.Range(0, spawn.randomizedPrefabs.Length)];
            var newObject = Instantiate(prefab, spawn.spawnPoint.parent);
            newObject.transform.localPosition = spawn.spawnPoint.localPosition;
            newObject.transform.localScale = spawn.spawnPoint.localScale;
            Destroy(potentialSpawns[chosenIndex].spawnPoint.gameObject);

            potentialSpawns[chosenIndex].spawned = true;
        }
    }

    [System.Serializable]
    public struct RandomSpawnLocation
    {
        public Transform spawnPoint;
        public GameObject[] randomizedPrefabs;

        public bool spawned;
    }
}
