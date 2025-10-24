using UnityEngine;
using Random = UnityEngine.Random;

public class BossGlueAttack : BossAttack
{
    [SerializeField] private GameObject gluePrefab;
    [SerializeField] private float checkRadius = 3;
    [SerializeField] private int minSoawns, maxSpawns;

    [SerializeField] private float spawnRadius;

    public override void UseAttack()
    {
        for (var i = 0; i < Random.Range(minSoawns, maxSpawns); i++)
        {
            var spawnPos = transform.position + (Vector3)Random.insideUnitCircle * Random.Range(1, spawnRadius);

            // Choose random spot
            var colliders = Physics2D.OverlapCircleAll(spawnPos, checkRadius);
            var spotHasGlue = false;

            foreach (var collider in colliders)
                if (collider.gameObject.layer == LayerMask.NameToLayer("BossGlueLayer") || collider.CompareTag("Boss"))
                {
                    spotHasGlue = true;
                    break;
                }

            // Check if spot has glue
            if (!spotHasGlue)
                // Spawn there if none
                Instantiate(gluePrefab, spawnPos, Quaternion.identity);
        }
    }
}