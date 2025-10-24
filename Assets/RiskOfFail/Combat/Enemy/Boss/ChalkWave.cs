using RiskOfFail.Combat;
using UnityEngine;

public class ChalkWave : MonoBehaviour
{
    public float damageAmount = 10;
    public float stunTime = 10;
    public float knockBack = 10;
    public KillFlag flag = KillFlag.AreaOfEffect;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.attachedRigidbody && col.TryGetComponent(out Alive a))
            col.attachedRigidbody.AddForce(
                (col.attachedRigidbody.transform.position - transform.position).normalized * knockBack,
                ForceMode2D.Impulse);

        if (col.TryGetComponent(out Enemy e)) e.GiveDamage(0, stunTime, flag);

        if (!col.CompareTag("Player")) return;

        if (col.TryGetComponent(out PlayerStatus p)) p.GiveDamage(damageAmount, stunTime, flag);
    }
}