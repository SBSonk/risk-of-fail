using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZSpawnContainer : MonoBehaviour
{
    [SerializeField] private Transform[] spawns;

    public void AddAllSpawns(ZSpawning2 spawner)
    {
        foreach (var spawn in spawns)
        {
            spawner.AddSpawn(spawn.position);
        }
    }
}
