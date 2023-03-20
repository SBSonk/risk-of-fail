using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class QuarterlyAssessment : Enemy
{
    protected override void Death()
    {
        // Drop drops
        DropObject drop = GetComponent<DropObject>();
        if (drop) drop.startDrop();

        // Give player score
        float lifetime = Time.time - spawnTime;
        LevelStats.main.GiveScore(killScore, lifetime);

        // Reduce alive enemies for the spawner`
        onEnemyDeath?.Invoke(type);

        LevelStats.main.EnemyKilled(); // Should use an event probably
        
        dead = true; 
        onDeath?.Invoke();
        
        Instantiate(deathSound);
        Destroy(healthParent);
    }

    public void TriggerDeath()
    {
        Death();
    }
}
