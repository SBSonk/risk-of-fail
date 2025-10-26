using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RiskOfFail.Combat;
using UnityEngine;

public class TrackingChalkAttackV2 : BossAttackV2
{
    public float boardWidth = 7;
    public float boardHeight = 2;
    public float bulletDistance = .1f;
    public int bulletAmount = 4;
    public float timeBetweenBullets = .25f;
    public float hoverTime = 2f;
    public float spawningTime = 2f;
    public TrackingRound bulletPrefab;

    private List<TrackingRound> bullets;

    public override IEnumerator Attack()
    {
        bullets = new List<TrackingRound>();
        var left = false;
        var index = 1;
        for (var i = 0; i < bulletAmount; i++)
        {
            // Spawn bullet on one of the transforms alternating
            var b = Instantiate(bulletPrefab, transform.position + new Vector3(0, boardHeight), Quaternion.identity);
            b.SetTarget(PlayerStatus.instance.transform);
            float dir = left ? -1 : 1;
            b.transform.DOMoveX(transform.position.x + boardWidth * dir + bulletDistance * (bulletAmount - i) * dir,
                spawningTime / bulletAmount * index);

            bullets.Add(b);
            if (!left) index++;
            left = !left;
        }

        yield return new WaitForSeconds(hoverTime);

        foreach (var b in bullets)
        {
            b.StartTimer();

            yield return new WaitForSeconds(timeBetweenBullets);
        }

        print("test");
        OnAttackEnd?.Invoke();
    }

    public override void CancelAttack()
    {
        if (bullets.Count == 0) return;

        for (var i = bullets.Count - 1; i < 0; i++) Destroy(bullets[i].gameObject);
    }
}