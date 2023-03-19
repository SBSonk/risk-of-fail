using UnityEngine;
using System.Collections;
using Pathfinding;
using UnityEngine.Events;

public class QuarterlyAssessmentAttack : WrittenWorksAttack
{
    public Animator EKUSPUROSION;
    public float dmg, stn;
    private Alive self;

    private float baseSpeed;

    public UnityEvent ExplosionStart;
    
    protected override void Start()
    {
        base.Start();
        
        self = GetComponent<Alive>();
        self.onDeath.AddListener(() =>
        {
            ExplosionStart?.Invoke();
            StartCoroutine(Explode());
        });
        baseSpeed = ai.maxSpeed;
    }

    protected override void AttackPlayer(Collider2D collision, float damage)
    {
        // Explosion
        self.onDeath?.Invoke();
    }

    protected override void Pushing()
    {
        ai.maxSpeed = baseSpeed * 1.5f;
        base.Pushing();
    }

    protected override void Pathing()
    {
        ai.maxSpeed = baseSpeed;
        base.Pathing();
    }

    IEnumerator Explode()
    {
        ai.enabled = false;
        yield return new WaitForSeconds(2);
        
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
        
        Destroy(gameObject);
    }
    // TODO: Determine if the enemy is stuck
    // Explode
}
