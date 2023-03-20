using UnityEngine;
using System.Collections;
using FirstGearGames.SmoothCameraShaker;
using Pathfinding;
using UnityEngine.Events;

public class QuarterlyAssessmentAttack : WrittenWorksAttack
{
    public bool exploding = false;
    public Animator EKUSPUROSION;
    public float dmg, stn;

    private float baseSpeed;

    public Transform explosionParticles;
    public UnityEvent ExplosionStart;

    public ShakeData explosionShake;
    public AudioSource explosionSound;
    
    protected override void Start()
    {
        base.Start();
        
        self = GetComponent<Enemy>();
        self.onDeath.AddListener(() =>
        {
            ExplosionStart?.Invoke();
            StartCoroutine(Explode());
        });
        baseSpeed = ai.maxSpeed;
    }

    protected override void AttackPlayer(Collider2D collision, float damage)
    {
        if (exploding) return;
        
        // Explosion
        var qa = self as QuarterlyAssessment;
        qa.TriggerDeath();
        exploding = true;
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
        
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        Instantiate(explosionSound);
        CameraShakerHandler.Shake(explosionShake);
        Destroy(gameObject);
    }
    // TODO: Determine if the enemy is stuck
    // Explode
}
