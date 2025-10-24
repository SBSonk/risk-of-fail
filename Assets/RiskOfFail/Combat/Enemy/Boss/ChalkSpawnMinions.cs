using System.Collections;
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

    private IEnumerator Attack(int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            // Choose random side (left or right)
            var chosenSideTransform = i % 2 == 0 ? leftSideTransform : rightSideTransform;

            // Calculate the position of the grenade based on the chosen side
            var spawnPosition = chosenSideTransform.position;

            // Spawn grenade at the calculated position
            var grenadeInstance = Instantiate(grenadePrefab, spawnPosition, Quaternion.identity);

            // Apply throw force to the grenade
            grenadeInstance.StartArc(GetRandomPosition(chosenSideTransform.position));

            yield return new WaitForSeconds(timeBetweenGrenades);
        }
    }

    private Vector3 GetRandomPosition(Vector3 chosenSidePosition)
    {
        var randomOffset = Vector3.zero;

        if (chosenSidePosition == leftSideTransform.position)
        {
            var randomXOffset = Random.Range(-4f, -1f); // Random X offset from -1 to 0 (left direction)
            var randomYOffset = Random.Range(-2f, 2f); // Random Y offset from -1 to 1 (both directions)

            randomOffset = new Vector3(randomXOffset, randomYOffset, 0f);
        }
        else if (chosenSidePosition == rightSideTransform.position)
        {
            var randomXOffset = Random.Range(1f, 4f); // Random X offset from 0 to 1 (right direction)
            var randomYOffset = Random.Range(-2f, 2f); // Random Y offset from -1 to 1 (both directions)

            randomOffset = new Vector3(randomXOffset, randomYOffset, 0f);
        }

        var targetPosition = chosenSidePosition + randomOffset;

        return targetPosition;
    }
}