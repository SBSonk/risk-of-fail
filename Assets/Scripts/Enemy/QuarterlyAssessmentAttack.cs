using UnityEngine;
using System.Collections;
using Pathfinding;

public class QuarterlyAssessmentAttack : WrittenWorksAttack
{
    public Animator EKUSPUROSION;
    public float dmg, stn;
    // TODO: attacks
    // trigger explosion when killed, stuck, or hit the player

    protected override void AttackPlayer(Collider2D collision)
    {
        // Normal Writtenwork Attack
        base.AttackPlayer(collision);

        // Explosion
        StartCoroutine(explode(collision));
    }

    IEnumerator explode(Collider2D collision)
    {
        //gameObject.GetComponent<Indicator>().play();
        EKUSPUROSION.GetComponent<Animator>().Play("Explosion Indicator");
        yield return new WaitForSeconds(2);
        print("boom");
        Collider2D[] raycastHit = Physics2D.OverlapCircleAll(transform.position, 5);
        foreach (Collider2D r in raycastHit)
        {
            print(r.name);
            var alive = r.GetComponent<Alive>();
            if (alive)
            {
                alive.GiveDamage(dmg, stn);
            }
        }
        yield break;

    }
    // TODO: Determine if the enemy is stuck
    // Explode
}
