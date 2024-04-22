using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ExplodingProjectile : Projectile
{
    public LayerMask enemyLayer, environmentLayer;
    public float explosionRadius = 3;
    public float explosionKnockback = 3;
    
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);
        List<Alive> exploded = new List<Alive>();
        if (hit.Length > 0)
        {
            foreach (var col in hit)
            {
                print(col.name);
                if (col.TryGetComponent(out Alive a))
                {
                    if (exploded.Contains(a)) continue;
                    if (Physics2D.Linecast(transform.position, a.transform.position, environmentLayer)) continue;

                    a.GiveDamage(damage, stunLength, KillFlag.Ranged);
                    a.GetComponent<Rigidbody2D>().AddForce((a.transform.position - transform.position).normalized * explosionKnockback, ForceMode2D.Impulse);
                    LevelStats.main.GiveDamage(damage);

                    exploded.Add(a);
                }
            }
        }

        enabled = false;
    }
}
