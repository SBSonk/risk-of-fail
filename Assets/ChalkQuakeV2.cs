using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChalkQuakeV2 : BossAttackV2
{
    public Rigidbody2D quakePrefab;
    
    public int minWaves = 3, maxWaves = 6;
    public float timeBetweenWaves = 2.5f;

    public int amountSpawned = 16;
    public float horizontalScale = 1;
    public float verticalScale = .75f;
    public Vector3 offset;
    public float quakeSpeed = 5;

    private void Start()
    {
        StartCoroutine(Attack());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + offset, .25f);
    }

    public void SpawnChalk()
    {
        for (int i = 0; i < amountSpawned; i++)
        {
            var radians = 2 * Mathf.PI / amountSpawned * i;

            /* Get the vector direction */
            var vertical = Mathf.Sin(radians) * verticalScale;
            var horizontal = Mathf.Cos(radians) * horizontalScale;

            var spawnDir = new Vector3(horizontal, vertical, 0);

            /* Get the spawn position */
            var spawnPos = transform.position + spawnDir; // Radius is just the distance away from the point

            /* Now spawn */
            var quake = Instantiate(quakePrefab, spawnPos + offset, quaternion.Euler(0, 0, radians));
            quake.velocity = spawnDir.normalized * quakeSpeed;
        }
    }

    public override IEnumerator Attack()
    {
        for (int i = 0; i < Random.Range(minWaves, maxWaves); i++)
        {
            SpawnChalk();
            yield return new WaitForSeconds(timeBetweenWaves);


            OnAttackEnd?.Invoke();
        }
    }
}
