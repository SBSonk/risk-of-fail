using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChalkQuakeAttack : BossAttack
{
    [SerializeField] private ChalkQuakeSegment segmentPrefab;
    [SerializeField] private float distanceBetweenSegments = 1;
    [SerializeField] private int maxSegments = 10;
    [SerializeField] private float setOffTime = 0.5f;
    private Collider2D _collider;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        InvokeRepeating("UseAttack", 0, 1.5f);
    }

    public override void UseAttack()
    {
        Vector3 playerPos = PlayerStatus.player.transform.position + new Vector3(0, 0.5f);
        Vector3 closestPoint = _collider.bounds.ClosestPoint(playerPos);
        Vector3 playerDir = (playerPos - closestPoint);
        for (int i = 0; i < maxSegments; i++)
        {
            var chalk = Instantiate(segmentPrefab);
            chalk.transform.position = closestPoint + (playerDir.normalized * 0.75f) + (playerDir.normalized * (distanceBetweenSegments * (i)));
            chalk.transform.up = playerDir.normalized;
            chalk.StartAttack(setOffTime * i);
        }
    }
}
