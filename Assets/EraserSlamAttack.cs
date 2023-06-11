using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraserSlamAttack : BossAttack
{
    [SerializeField] private LerpFollow eraserPrefab;

    private Transform player;

    private void Start()
    {
        player = PlayerStatus.player.transform;
    }

    public override void UseAttack()
    {
        Instantiate(eraserPrefab).target = player;
    }
}
