using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChalkQuakeAttack : BossAttack
{
    [SerializeField] private ChalkQuakeSegment segmentPrefab;
    [SerializeField] private int amountOfAttacks = 5;
    [SerializeField] private float timeBetweenAttacks = 1;
    [SerializeField] private float distanceBetweenSegments = 1;
    [SerializeField] private int maxSegments = 10;
    [SerializeField] private float setOffTime = 0.5f;
    private Collider2D _collider;

    [SerializeField] private Spawning2 spawner;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    public override void UseAttack()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        spawner.StartSpawner();
        
        for (int i = 0; i < amountOfAttacks; i++)
        {
            Vector3 playerPos = PlayerStatus.player.transform.position + new Vector3(0, 0.5f);
            Vector3 closestPoint = _collider.bounds.ClosestPoint(playerPos);
            Vector3 playerDir = (playerPos - closestPoint);
            for (int o = 0; o < maxSegments; o++)
            {
                var chalk = Instantiate(segmentPrefab);
                chalk.transform.position = closestPoint + (playerDir.normalized * 0.75f) + (playerDir.normalized * (distanceBetweenSegments * (o)));
                chalk.transform.up = playerDir.normalized;
                chalk.StartAttack(setOffTime * o);
            }

            yield return new WaitForSeconds(timeBetweenAttacks);
        }

        spawner.StopSpawner();
    }
}
