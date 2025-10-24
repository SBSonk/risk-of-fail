using DG.Tweening;
using RiskOfFail.Combat;
using UnityEngine;

public class BossRush : MonoBehaviour
{
    public SpriteRenderer warningSprite;
    public float warningTime = 3f;

    [Header("Attack")] public float damageAmount = 15;
    public float stunLength = 1;
    public KillFlag killFlag = KillFlag.AreaOfEffect;
    public Vector3 offset, size;

    public float knockbackStrength = 25f;

    [Header("Movement")] public Vector3 amountToMove;
    public float speed = 10;


    private void Update()
    {
        var c = Physics2D.OverlapBoxAll(transform.position + offset, size, 0);

        for (var i = 0; i < c.Length; i++)
        {
            if (c[i].gameObject.layer == LayerMask.NameToLayer("PlayerImmune")) return;

            if (c[i].attachedRigidbody)
            {
                c[i].attachedRigidbody.linearVelocity = Vector2.zero;
                c[i].attachedRigidbody
                    .AddForce((c[i].transform.position - (transform.position + offset)).normalized * knockbackStrength,
                        ForceMode2D.Impulse);
            }

            if (c[i].TryGetComponent(out Enemy e)) e.GiveDamage(0, stunLength, killFlag);

            if (!c[i].CompareTag("Player")) return;

            if (c[i].TryGetComponent(out PlayerStatus p)) p.GiveDamage(damageAmount, stunLength, killFlag);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + offset, size);

        Gizmos.DrawSphere(transform.position + amountToMove, 1f);
    }

    public void StartMove()
    {
        StartCoroutine(HelperFunctions.Flicker(warningSprite, warningTime,
            () => transform.DOMove(transform.position + amountToMove, speed).SetSpeedBased(true)));
    }
}