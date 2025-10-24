using Pathfinding;
using RiskOfFail.Combat;
using RiskOfFail.Combat.Enums;
using UnityEngine;

namespace RiskOfFail.AI.Animators
{
    public class QuizAnimator : MonoBehaviour
    {
        [SerializeField] private Transform sprite;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private ParticleSystem deathParticles;

        public bool canSwitchAnimations = true;

        protected Animator animator;

        private float currentAnimation = 0;
        protected Directions dirFacing = Directions.down;
        protected Enemy enemy;
        protected AIPath pathing;

        protected virtual void Start()
        {
            animator = GetComponent<Animator>();
            enemy = GetComponent<Enemy>();
            pathing = GetComponent<AIPath>();

            enemy.onHit.AddListener(StunAnimation);
            enemy.onEnemyDeath.AddListener(DeathAnimation);
        }

        protected virtual void Update()
        {
            // Sprites
            sprite.position = transform.position + new Vector3(0, -0.75f);
            sprite.rotation = Quaternion.identity;

            if (!canSwitchAnimations) return;

            if (enemy.canSeePlayer)
                dirFacing = VectorToDir((pathing.destination - transform.position).normalized);
            else
                dirFacing = VectorToDir(pathing.velocity.normalized);

            if (pathing.velocity.magnitude > 0) WalkAnimation();
            else StandAnimation();
        }

        protected virtual void WalkAnimation()
        {
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

        protected virtual void StandAnimation()
        {
            spriteRenderer.flipX = dirFacing == Directions.left;

            switch (dirFacing)
            {
                case Directions.up:
                    animator.Play("stand_u");
                    break;

                case Directions.right:
                    animator.Play("stand_r");
                    break;

                case Directions.down:
                    animator.Play("stand_d");
                    break;

                case Directions.left:
                    animator.Play("stand_r");
                    break;
            }
        }

        public virtual void ShootAnimation()
        {
            if (!canSwitchAnimations) return;

            switch (dirFacing)
            {
                case Directions.up:
                    animator.Play("attack_u");
                    break;

                case Directions.right:
                    animator.Play("attack_r");
                    break;

                case Directions.down:
                    animator.Play("attack_d");
                    break;

                case Directions.left:
                    animator.Play("attack_r");
                    break;
            }

            CancelInvoke("EnableAnimations");
            canSwitchAnimations = false;
            Invoke("EnableAnimations", 1f);
        }

        public void DeathAnimation(EnemyType _)
        {
            deathParticles.transform.SetParent(null);
            Destroy(deathParticles.gameObject, 2f);
            deathParticles.Play();
        }

        private void StunAnimation(float duration, DamageTypeFlag _)
        {
            if (!canSwitchAnimations) return;

            switch (dirFacing)
            {
                case Directions.up:
                    animator.Play("hurt_u");
                    break;

                case Directions.right:
                    animator.Play("hurt_r");
                    break;

                case Directions.down:
                    animator.Play("hurt_d");
                    break;

                case Directions.left:
                    animator.Play("hurt_r");
                    break;
            }

            CancelInvoke("EnableAnimations");
            canSwitchAnimations = false;
            Invoke("EnableAnimations", 0.5f);
        }

        private void EnableAnimations()
        {
            canSwitchAnimations = true;
        }

        protected Directions VectorToDir(Vector2 input)
        {
            var final = dirFacing;

            if (input.x > 0.5f) final = Directions.right;
            else if (input.x < -0.5f) final = Directions.left;
            else if (input.y > 0.5f) final = Directions.up;
            else if (input.y < -0.5f) final = Directions.down;

            return final;
        }
    }
}