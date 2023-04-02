using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class QuarterlyAssessment : Enemy
{
    protected override void Death(KillFlag flag)
    {
        // Drop drops
        DropObject drop = GetComponent<DropObject>();
        if (drop) drop.startDrop();

        // Give player score
        float lifetime = Time.time - spawnTime;
        LevelStats.main.GiveScore(killScore, lifetime, flag);

        // Reduce alive enemies for the spawner`
        onEnemyDeath?.Invoke(type);

        LevelStats.main.EnemyKilled(flag); // Should use an event probably
        
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
