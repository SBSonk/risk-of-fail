using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChalkSpawnMinions : BossAttack
{
    [SerializeField] private Throwable grenadePrefab;
    [SerializeField] private Transform leftSideTransform;
    [SerializeField] private Transform rightSideTransform;
    [SerializeField] private int minAmount = 1;
    [SerializeField] private int maxAmount = 3;
    [SerializeField] private float timeBetweenGrenades = .5f;

    public override void UseAttack()
    {
        StartCoroutine(Attack(Random.Range(minAmount, maxAmount)));
    }

    IEnumerator Attack(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            // Choose random side (left or right)
            Transform chosenSideTransform = i % 2 == 0 ? leftSideTransform : rightSideTransform;
            
            // Calculate the position of the grenade based on the chosen side
            Vector3 spawnPosition = chosenSideTransform.position;

            // Spawn grenade at the calculated position
            Throwable grenadeInstance = Instantiate(grenadePrefab, spawnPosition, Quaternion.identity);

            // Apply throw force to the grenade
            grenadeInstance.StartArc(GetRandomPosition(chosenSideTransform.position));

            yield return new WaitForSeconds(timeBetweenGrenades);
        }
    }

    private Vector3 GetRandomPosition(Vector3 chosenSidePosition)
    {
        Vector3 randomOffset = Vector3.zero;

        if (chosenSidePosition == leftSideTransform.position)
        {
            float randomXOffset = Random.Range(-4f, -1f); // Random X offset from -1 to 0 (left direction)
            float randomYOffset = Random.Range(-2f, 2f); // Random Y offset from -1 to 1 (both directions)

            randomOffset = new Vector3(randomXOffset, randomYOffset, 0f);
        }
        else if (chosenSidePosition == rightSideTransform.position)
        {
            float randomXOffset = Random.Range(1f, 4f); // Random X offset from 0 to 1 (right direction)
            float randomYOffset = Random.Range(-2f, 2f); // Random Y offset from -1 to 1 (both directions)

            randomOffset = new Vector3(randomXOffset, randomYOffset, 0f);
        }

        Vector3 targetPosition = chosenSidePosition + randomOffset;

        return targetPosition;
    }
}
