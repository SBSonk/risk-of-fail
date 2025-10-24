using System.Collections;
using Pathfinding;
using RiskOfFail.Combat.Enums;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace RiskOfFail.Combat
{
    public class Enemy : Alive
    {
        public static float globalEnemyHealthScale = 1;

        public bool canSeePlayer;

        [Header("Enemy")] [SerializeField] protected EnemyType type = EnemyType.WrittenWorks;
        [SerializeField] protected int killScore;

        [Header("Health Bar")] [SerializeField]
        private GameObject hBarPrefab;

        [SerializeField] private Vector3 hbarOffset = new(0, 1.25f);

        [Header("Movement")] [SerializeField] private float minSpeed = 10;
        [SerializeField] private float maxSpeed = 15;
        [SerializeField] private float speed = 10;
        [SerializeField] private float hbarLerp = 0.5f, StunRecoverTime = 0.125f;

        public MonoBehaviour attackScript;
        public Rigidbody2D rb;

        public UnityEvent<EnemyType> onEnemyDeath;
        private Transform healthBar;

        private SpriteRenderer[] healthBarSprites;

        protected GameObject healthParent;
        protected AIPath pathAI;
        protected float spawnTime;

        protected void Awake()
        {
            health *= globalEnemyHealthScale;
            maxHealth *= globalEnemyHealthScale;

            // Initialize AI
            pathAI = GetComponent<AIPath>();
            healthParent = Instantiate(hBarPrefab, transform);
            healthBar = healthParent.transform.GetChild(0);
            rb = GetComponent<Rigidbody2D>();

            // Randomize speed
            speed = Random.Range(minSpeed, maxSpeed);
            if (pathAI) pathAI.maxSpeed = speed;

            // Keep track of lifetime
            spawnTime = Time.time;

            healthBarSprites = healthParent.GetComponentsInChildren<SpriteRenderer>();
            if (attackScript) attackScript.enabled = true;
        }

        private void Update()
        {
            if (!PlayerStatus.IsAlive) return;

            // Check if there is environment collision in the way
            canSeePlayer = !Physics2D.Linecast(transform.position, PlayerStatus.player.transform.position,
                LayerMask.NameToLayer("Environment"));

            // Make health bar follow enemy
            if (healthParent)
            {
                healthParent.transform.position = Vector3.Lerp(healthParent.transform.position,
                    transform.position + hbarOffset, hbarLerp);

                // Healthbar animation  
                healthBar.transform.localScale = Vector3.Lerp(healthBar.transform.lossyScale,
                    new Vector3(1 * (health / maxHealth), 1, 1), hbarLerp);

                healthParent.transform.rotation = Quaternion.identity;
            }
        }

        public void SetSpeedMultiplier(float multiplier)
        {
            pathAI.maxSpeed = speed * multiplier;
        }

        public void ResetSpeed()
        {
            pathAI.maxSpeed = speed;
        }


        private IEnumerator StunRecover(float time)
        {
            if (pathAI)
            {
                var startSpeed = pathAI.maxSpeed;

                float t = 0;
                while (t < time)
                {
                    pathAI.maxSpeed = Mathf.Lerp(startSpeed, speed, t / time);
                    t += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }

                pathAI.maxSpeed = speed;
            }
        }

        private IEnumerator HealthBarFade(float startOpacity, float finalOpacity, float t)
        {
            var opacity = startOpacity;
            float time = 0;
            while (time < t)
            {
                opacity = Mathf.Lerp(opacity, finalOpacity, time / t);

                for (var i = 0; i < healthBarSprites.Length; i++)
                {
                    var c = healthBarSprites[i].color;
                    c.a = opacity;
                    healthBarSprites[i].color = c;
                }

                yield return new WaitForFixedUpdate();
                time += Time.fixedDeltaTime;
            }
        }

        protected override void OnDamage(float damage)
        {
            base.OnDamage(damage);

            if (healthParent && healthBar)
            {
                CancelInvoke("HideHealthBar");
                healthParent.SetActive(true);
                StartCoroutine(HealthBarFade(0, 1, 0.3f));
                Invoke("HideHealthBar", 2.5f);
            }

            onHit?.Invoke(damage, DamageTypeFlag.Melee);
        }

        private void HideHealthBar()
        {
            StartCoroutine(HealthBarFade(1, 0, 0.3f));
        }

        protected override void Death(DamageTypeFlag flag)
        {
            // Destroy health bar
            Destroy(healthParent);

            // Drop drops
            var drop = GetComponent<DropObject>();
            if (drop) drop.startDrop();

            // Give player score
            var lifetime = Time.time - spawnTime;
            LevelStats.main.GiveScore(killScore, lifetime, flag);

            // Reduce alive enemies for the spawner
            onEnemyDeath?.Invoke(type);

            LevelStats.main.EnemyKill(flag); // Should use an event probably

            base.Death(flag);
        }

        public override void Stun(float duration)
        {
            // Can't get stunned twice
            if (stunned) return;

            stunned = true;

            if (pathAI)
            {
                pathAI.canMove = false;
                pathAI.maxSpeed = 0;
            }

            if (attackScript) attackScript.enabled = false;

            StartCoroutine(clearStun(duration));
        }

        protected IEnumerator clearStun(float time)
        {
            yield return new WaitForSeconds(time);

            if (pathAI) pathAI.canMove = true;
            stunned = false;
            if (attackScript) attackScript.enabled = true;

            // Reset rigidbody velocities
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0;

            StartCoroutine(StunRecover(StunRecoverTime));
        }
    }
}