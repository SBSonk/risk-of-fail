using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;

public class BossRoomRunPhase : MonoBehaviour
{
    public BossEnemy boss;


    [Header("Tracking Chalk")] 
    public float boardWidth = 7;
    public float boardHeight = 2;
    public float bulletDistance = .1f;
    public int bulletAmount = 4;
    public float timeBetweenBullets = .25f;
    public float hoverTime = 2f;
    public float spawningTime = 2f;
    public TrackingRound bulletPrefab;


    private void Start()
    {
        StartCoroutine(TrackingChalkAttack());
    }

    IEnumerator TrackingChalkAttack()
    {
        List<TrackingRound> bullets = new List<TrackingRound>();
        bool left = false;
        int index = 1;
        for (int i = 0; i < bulletAmount; i++)
        {
            // Spawn bullet on one of the transforms alternating
            TrackingRound b = Instantiate(bulletPrefab, transform.position + new Vector3(0, boardHeight), quaternion.identity);
            b.SetTarget(PlayerStatus.player.transform);
            float dir = (left ? -1 : 1);
            b.transform.DOMoveX(transform.position.x + (boardWidth * dir) + (bulletDistance * (bulletAmount-i) * dir), spawningTime / bulletAmount * index);

            bullets.Add(b);
            if (!left) index++;
            left = !left;
            
        }

        yield return new WaitForSeconds(hoverTime);

        foreach (var b in bullets)
        {
            b.StartTimer();

            yield return new WaitForSeconds(timeBetweenBullets);
        }
    }
}
