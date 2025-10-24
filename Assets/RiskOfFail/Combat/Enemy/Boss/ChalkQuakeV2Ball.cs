using RiskOfFail.Combat;
using RiskOfFail.Combat.Enums;
using UnityEngine;

public class ChalkQuakeV2Ball : MonoBehaviour
{
    [Header("Attack")] public float damageAmount = 15;
    public float stunLength = 1;
    public DamageTypeFlag killFlag = DamageTypeFlag.AreaOfEffect;
    public float damageRadius = .25f;

    public float knockbackStrength = 25f;

    private void Update()
    {
        var c = Physics2D.OverlapCircleAll(transform.position, damageRadius);

        for (var i = 0; i < c.Length; i++)
        {
            if (c[i].gameObject.layer == LayerMask.NameToLayer("Environment")) Destroy(gameObject);

            if (c[i].gameObject.layer == LayerMask.NameToLayer("PlayerImmune")) return;

            if (c[i].attachedRigidbody)
            {
                c[i].attachedRigidbody.linearVelocity = Vector2.zero;
                c[i].attachedRigidbody
                    .AddForce((c[i].transform.position - transform.position).normalized * knockbackStrength,
                        ForceMode2D.Impulse);
            }

            if (c[i].TryGetComponent(out Enemy e)) e.GiveDamage(0, stunLength, killFlag);

            if (!c[i].CompareTag("Player")) return;

            if (c[i].TryGetComponent(out PlayerStatus p)) p.GiveDamage(damageAmount, stunLength, killFlag);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, damageRadius);
    }
}