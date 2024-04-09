using System;
using System.Collections;
using System.Collections.Generic;
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
                if (c.TryGetComponent(out Enemy e))
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
            rb.velocity = transform.right * homingStrength;
        }
        
        Vector2 dirTowards = (transform.position + (Vector3) rb.velocity - transform.position).normalized;
        transform.right = Vector3.Lerp(transform.right, dirTowards, Time.deltaTime * homingStrength);
    }
}
