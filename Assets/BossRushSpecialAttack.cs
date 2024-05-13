using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossRushSpecialAttack : BossAttackV2
{
    public Transform bossRushPrefab;
    public int minAmount = 2, maxAmount = 4;
    
    
    public float rushSpeed = 10;

    public Vector3 centerOffset;
    public float stageWidth = 20;
    public float stageHeight = 20;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + centerOffset, new Vector3(stageWidth, stageHeight));
    }

    public override IEnumerator Attack()
    {
        for (int i = 0; i < Random.Range(minAmount,maxAmount); i++)
        {
            Transform rush = Instantiate(bossRushPrefab, transform.position + centerOffset, quaternion.identity);
        }

        yield return null;
    }
}
