using RiskOfFail.Combat;
using UnityEngine;

public class Grenade : Throwable
{
    [SerializeField] protected float baseDamage = 10f;
    [SerializeField] protected float stunTime = 1f;

    protected override void Explode()
    {
        print("bboom");
        var raycastHit = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var r in raycastHit)
            if (r.TryGetComponent<Enemy>(out var enemy))
                enemy.GiveDamage(baseDamage, stunTime, KillFlag.AreaOfEffect);
    }
}