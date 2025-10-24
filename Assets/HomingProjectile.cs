
using System;
using System.Collections;
using System.Collections.Generic;
using RiskOfFail.Combat;
using UnityEngine;

public class HomingProjectile : Projectile
{
    public float homingRange = 3;
    public float homingStrength = 10;
    public LayerMask layermask;
    
    public Transform lockedEnemy = null;
    private bool locked = false;

    private void Update()
    {
        if (!locked)
        {
            // check for area
            
            // if found, lock on

            var col = Physics2D.OverlapCircleAll(transform.position, homingRange, layermask);

            foreach (Collider2D c in col)
            {
                if (c.TryGetComponent(out Enemy e) || c.TryGetComponent(out BossEnemy b))
                {
                    lockedEnemy = c.transform;
                    locked = true;

                    break;
                }
            }
        }

        if (lockedEnemy)
        {
            // go towards
            transform.right = Vector3.Lerp(transform.right, (lockedEnemy.position - transform.position).normalized, Time.deltaTime * homingStrength);
            rb.linearVelocity = transform.right * homingStrength;
        }
    }
}
