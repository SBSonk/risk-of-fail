using System.Collections;
using RiskOfFail.Combat;
using UnityEngine;

public class ChalkQuakeAttack : BossAttack
{
    [SerializeField] private ChalkQuakeSegment segmentPrefab;
    [SerializeField] private int amountOfAttacks = 5;
    [SerializeField] private float timeBetweenAttacks = 1;
    [SerializeField] private float distanceBetweenSegments = 1;
    [SerializeField] private int maxSegments = 10;
    [SerializeField] private float setOffTime = 0.5f;

    [SerializeField] private Spawning2 spawner;
    private Collider2D _collider;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    public override void UseAttack()
    {
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        spawner.StartSpawner();

        for (var i = 0; i < amountOfAttacks; i++)
        {
            var playerPos = PlayerStatus.instance.transform.position + new Vector3(0, 0.5f);
            var closestPoint = _collider.bounds.ClosestPoint(playerPos);
            var playerDir = playerPos - closestPoint;
            for (var o = 0; o < maxSegments; o++)
            {
                var chalk = Instantiate(segmentPrefab);
                chalk.transform.position = closestPoint + playerDir.normalized * 0.75f +
                                           playerDir.normalized * (distanceBetweenSegments * o);
                chalk.transform.up = playerDir.normalized;
                chalk.StartAttack(setOffTime * o);
            }

            yield return new WaitForSeconds(timeBetweenAttacks);
        }

        spawner.StopSpawner();
    }
}