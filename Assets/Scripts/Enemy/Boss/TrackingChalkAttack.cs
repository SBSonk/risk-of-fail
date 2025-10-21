using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingChalkAttack : BossAttack
{
    [SerializeField] private TrackingRound bulletPrefab;
    [SerializeField] private int amountPerAttack = 10;
    [SerializeField] private float timeBetweenBullets = 0.25f;
    [SerializeField] private Transform leftTransform;
    [SerializeField] private Transform rightTransform;
    [SerializeField] private ChalkSpawnMinions minionSpawner;
    
    private Transform player;

    private void Start()
    {
        player = PlayerStatus.player.transform;
    }

    public override void UseAttack()
    {
        StartCoroutine(Attack(amountPerAttack));
    }

    IEnumerator Attack(int amount)
    {
        bool spawnOnLeft = true;
        
        // spawn on middle, stop 5s

        for (int i = 0; i < amount; i++)
        {
            // Spawn bullet on one of the transforms alternating
            Transform spawnTransform = spawnOnLeft ? leftTransform : rightTransform;
            TrackingRound bullet = Instantiate(bulletPrefab, spawnTransform.position, spawnTransform.rotation);
            bullet.SetTarget(player);

            spawnOnLeft = !spawnOnLeft;

            yield return new WaitForSeconds(timeBetweenBullets);

            if (i == amount / 2)
            {
                minionSpawner.UseAttack();

                yield return new WaitForSeconds(2);
                
                minionSpawner.UseAttack();

                yield return new WaitForSeconds(2);
            }
        }
    }
}
