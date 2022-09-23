using UnityEngine;

public class QuarterlyAssessmentAttack : WrittenWorksAttack
{
    // TODO: attacks
    // trigger explosion when killed, stuck, or hit the player
    protected override void AttackPlayer(Collider2D collision)
    {
        // Normal Writtenwork Attack
        base.AttackPlayer(collision);

        // Explosion
        explode(collision);
    }

    void explode(Collider2D collision)
    {
        // PUT CODE HERE
        // Make the enemy explode
    }

    // TODO: Determine if the enemy is stuck
    // Explode
}
