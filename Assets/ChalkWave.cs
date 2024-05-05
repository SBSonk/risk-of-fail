using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChalkWave : MonoBehaviour
{
    public float damageAmount = 10;
    public float stunTime = 10;
    public KillFlag flag = KillFlag.AreaOfEffect;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        if (col.TryGetComponent(out PlayerStatus p))
        {
            p.GiveDamage(damageAmount, stunTime, flag);
        }
    }
}
