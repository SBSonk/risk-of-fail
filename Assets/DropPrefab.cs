using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPrefab : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int minAmountToSpawn = 1, maxAmountToSpawn = 1;
    [SerializeField] private Vector2 minOffset, maxOffset;
    [SerializeField] private Vector2 minSpawnScale, maxSpawnScale;
    [SerializeField] private float minAngle = 0, maxAngle = 360;

    public void SpawnPrefab(Vector3 guidePosition)
    {
        for (int i = 0; i < Random.Range(minAmountToSpawn, maxAmountToSpawn + 1); i++)
        {
            var t = Instantiate(prefab).transform;
            t.position = transform.position +
                         new Vector3(Random.Range(minOffset.x, maxOffset.x), Random.Range(minOffset.y, maxOffset.y));
            
            t.localScale = new Vector3(Random.Range(minSpawnScale.x, maxSpawnScale.x), Random.Range(minSpawnScale.y, maxSpawnScale.y));
            t.rotation = Quaternion.Euler(0, 0, Random.Range(minAngle, maxAngle));
        }
    }
}
