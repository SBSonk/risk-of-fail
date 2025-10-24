using System.Collections;
using FirstGearGames.SmoothCameraShaker;
using RiskOfFail.Combat;
using RiskOfFail.Combat.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace RiskOfFail.AI.Behaviors
{
    public class QuarterlyAssessmentAttack : WrittenWorksAttack
    {
        public bool exploding;
        public Animator EKUSPUROSION;
        public float dmg, stn;

        public Transform explosionParticles;
        public UnityEvent ExplosionStart;

        public ShakeData explosionShake;
        public AudioSource explosionSound;

        private float baseSpeed;

        protected override void Start()
        {
            base.Start();

            self = GetComponent<Enemy>();
            self.onDeath.AddListener(_ =>
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

        private IEnumerator Explode()
        {
            ai.enabled = false;
            yield return new WaitForSeconds(2);

            var raycastHit = Physics2D.OverlapCircleAll(transform.position, 5);
            foreach (var r in raycastHit)
            {
                var objectHit = Physics2D.Linecast(transform.position, r.transform.position, hitFilter.layerMask);
                if (objectHit && objectHit.collider.CompareTag("Wall")) continue;

                var alive = r.GetComponent<Alive>();
                if (alive) alive.GiveDamage(dmg, stn, DamageTypeFlag.AreaOfEffect);
            }

            Instantiate(explosionParticles, transform.position, Quaternion.identity);
            Instantiate(explosionSound);
            CameraShakerHandler.Shake(explosionShake);
            Destroy(gameObject);
        }
        // TODO: Determine if the enemy is stuck
        // Explode
    }
}