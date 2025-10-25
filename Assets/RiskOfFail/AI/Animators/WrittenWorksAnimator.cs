using Pathfinding;
using RiskOfFail.AI.Behaviors;
using RiskOfFail.Combat;
using RiskOfFail.Combat.Enums;
using UnityEngine;

namespace RiskOfFail.AI.Animators
{
    public class WrittenWorksAnimator : MonoBehaviour
    {
        [SerializeField] private Transform sprite;
        [SerializeField] private Animator animator;
        [SerializeField] private ParticleSystem deathParticles, dashParticles;

        [SerializeField] private SpriteRenderer dashIndicator;
        [SerializeField] private SpriteRenderer[] arrowIndicator;

        public bool canSwitchAnimations = true;
        private Directions dirFacing = Directions.down;
        private Enemy enemy;
        private WrittenWorksAttack enemyAttack;
        private AIPath pathing;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            enemy = GetComponent<Enemy>();
            enemyAttack = GetComponent<WrittenWorksAttack>();
            pathing = GetComponent<AIPath>();
            spriteRenderer = sprite.GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            //enemy.onHit.AddListener(StunAnimation);
            enemyAttack.OnSwipeAttack.AddListener(SwipeAnimation);
            enemy.onDeath.AddListener(DeathAnimation);
            //enemyAttack.OnDashAttack.AddListener(DashAnimation);
            //enemyAttack.OnDashStart.AddListener(DashStart);
            //enemyAttack.OnDashEnd.AddListener(DashFinish);
            //enemyAttack.OnDashCancel.AddListener(DashCancel);
        }

        private void Update()
        {
            dashIndicator.transform.position = transform.position + Vector3.up * 1.5f;
            dashIndicator.transform.rotation = Quaternion.identity;


            // Sprites
            sprite.rotation = Quaternion.identity;

            if (!canSwitchAnimations) return;

            /*if (enemy.canSeePlayer)
                dirFacing = VectorToDir((pathing.destination - transform.position).normalized);
            else*/
            dirFacing = VectorToDir(pathing.velocity.normalized);

            spriteRenderer.flipX = dirFacing == Directions.left;

            switch (dirFacing)
            {
                case Directions.up:
                    animator.Play("walk_u");
                    break;

                case Directions.right:
                    animator.Play("walk_r");
                    break;

                case Directions.down:
                    animator.Play("walk_d");
                    break;

                case Directions.left:
                    animator.Play("walk_r");
                    break;
            }
        }

        private Directions VectorToDir(Vector2 input)
        {
            var final = dirFacing;

            if (input.x > 0.5f) final = Directions.right;
            else if (input.x < -0.5f) final = Directions.left;
            else if (input.y > 0.5f) final = Directions.up;
            else if (input.y < -0.5f) final = Directions.down;

            return final;
        }

        private void StunAnimation(float duration, DamageTypeFlag _)
        {
            switch (dirFacing)
            {
                case Directions.up:
                    animator.Play("ww_hurt_u");
                    break;

                case Directions.right:
                    animator.Play("ww_hurt_r");
                    break;

                case Directions.down:
                    animator.Play("ww_hurt_d");
                    break;

                case Directions.left:
                    animator.Play("ww_hurt_r");
                    break;
            }

            CancelInvoke("EnableAnimations");
            canSwitchAnimations = false;
            Invoke("EnableAnimations", 0.5f);
        }

        private void SwipeAnimation()
        {
            switch (dirFacing)
            {
                case Directions.up:
                    animator.Play("ww_swipe_u");
                    break;

                case Directions.right:
                    animator.Play("ww_swipe_r");
                    break;

                case Directions.down:
                    animator.Play("ww_swipe_d");
                    break;

                case Directions.left:
                    animator.Play("ww_swipe_r");
                    break;
            }

            CancelInvoke("EnableAnimations");
            canSwitchAnimations = false;
            Invoke("EnableAnimations", 0.5f);
        }

        private void DashAnimation()
        {
            StartCoroutine(HelperFunctions.Fade(dashIndicator, Color.clear, Color.white, 0.1f));

            StartCoroutine(HelperFunctions.Fade(arrowIndicator[0], Color.clear, Color.white, 0.1f));
            ;
            StartCoroutine(HelperFunctions.Fade(arrowIndicator[1], Color.clear, Color.white, 1.05f));

            dashParticles.Play();
        }

        private void DashStart()
        {
            StartCoroutine(HelperFunctions.Fade(dashIndicator, Color.white, Color.clear, 0.1f));
            foreach (var s in arrowIndicator) StartCoroutine(HelperFunctions.Fade(s, Color.white, Color.clear, 0.1f));
        }

        private void DashFinish()
        {
            dashParticles.Stop();
        }

        private void DashCancel()
        {
            StopAllCoroutines();

            if (dashIndicator.color != Color.clear)
            {
                StartCoroutine(HelperFunctions.Fade(dashIndicator, Color.white, Color.clear, 0.1f));
                foreach (var s in arrowIndicator)
                    StartCoroutine(HelperFunctions.Fade(s, Color.white, Color.clear, 0.1f));
            }

            dashParticles.Clear();
            dashParticles.Stop();

            canSwitchAnimations = true;
        }

        private void EnableAnimations()
        {
            canSwitchAnimations = true;
        }

        public void DeathAnimation(DamageTypeFlag _)
        {
            deathParticles.transform.SetParent(null);
            Destroy(deathParticles.gameObject, 2f);
            deathParticles.Play();
        }
    }
}