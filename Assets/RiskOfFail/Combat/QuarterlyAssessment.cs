using UnityEngine;

namespace RiskOfFail.Combat
{
    public class QuarterlyAssessment : Enemy
    {
        protected override void Death(KillFlag flag)
        {
            // Drop drops
            var drop = GetComponent<DropObject>();
            if (drop) drop.startDrop();

            // Give player score
            var lifetime = Time.time - spawnTime;
            LevelStats.main.GiveScore(killScore, lifetime, flag);

            // Reduce alive enemies for the spawner`
            onEnemyDeath?.Invoke(type);

            LevelStats.main.EnemyKill(flag); // Should use an event probably

            dead = true;
            onDeath?.Invoke(flag);

            Instantiate(deathSound);
            Destroy(healthParent);
            StopAllCoroutines();
        }

        public void TriggerDeath()
        {
            Death(KillFlag.Self);
        }
    }
}