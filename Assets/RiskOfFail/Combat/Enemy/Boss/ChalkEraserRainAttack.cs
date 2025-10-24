using System.Collections;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChalkEraserRainAttack : BossAttackV2
{
    [Header("Attack")] public Collider2D[] spawnArea;

    public GameObject[] prefabs;
    public float padding = 1;
    public int minSpawns = 2, maxSpawns = 4;
    public float timeBetweenSpawns = .25f;
    public float hoverTime = 2f;

    [Header("Animation")] public GameObject staticPrefab;

    public float maxX = 15;
    public float minFloatTime = 2f;
    public float maxFloatTime = 4f;
    public float minTimeBetweenFloats = .25f;
    public float maxTimeBetweenFloats = .5f;

    public override IEnumerator Attack()
    {
        var range = Random.Range(minSpawns, maxSpawns);
        // Animation
        for (var i = 0; i < range; i++)
        {
            var newStaticEraser = Instantiate(staticPrefab,
                transform.position + new Vector3(Random.Range(-maxX, maxX), -40),
                quaternion.identity);

            newStaticEraser.transform.DOMoveY(newStaticEraser.transform.position.y + 60,
                Random.Range(minFloatTime, maxFloatTime));
            yield return new WaitForSeconds(Random.Range(minTimeBetweenFloats, maxTimeBetweenFloats));
        }

        OnAttackEnd?.Invoke();
        yield return new WaitForSeconds(hoverTime);

        // Spawn Attacks 
        for (var i = 0; i < range; i++)
        {
            var newEraser = Instantiate(prefabs[Random.Range(0, prefabs.Length)]);
            newEraser.transform.position = ChooseSpawnPoint();

            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    public Vector3 ChooseSpawnPoint()
    {
        var col = spawnArea[Random.Range(0, spawnArea.Length)];

        var spawnPoint = new Vector3(Random.Range(col.bounds.min.x + padding, col.bounds.max.x - padding),
            Random.Range(col.bounds.min.y + padding, col.bounds.max.y - padding));

        return spawnPoint;
    }
}