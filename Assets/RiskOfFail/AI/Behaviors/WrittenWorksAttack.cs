using System.Collections;
using Pathfinding;
using RiskOfFail.Combat;
using RiskOfFail.Combat.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace RiskOfFail.AI.Behaviors
{
    // I should really make this inherit QuizAI or something... too much work
    public class WrittenWorksAttack : MonoBehaviour
    {
        [SerializeField] private DamageSource damage;

        [SerializeField] private float wakeDistance = 10;
        [SerializeField] private float stunLength, knockbackAmount, reboundLength;
        [SerializeField] private float rushDistance = 5f, attackRange = 5f;

        public ContactFilter2D hitFilter;
        public AIMode mode = AIMode.pathing;
        public float minDashDistance = 5f, maxDashDistance = 6f, minDashCooldown = 5f, maxDashCooldown = 10f;
        public UnityEvent OnSwipeAttack, OnDashAttack, OnDashStart, OnDashEnd, OnDashCancel;

        public Transform dashIndicator;
        protected AIPath ai;
        private bool attacking, dashing, canDash = true, dashQueued, canAttack = true;
        private float defaultSpeed;

        private float distanceToPlayer;

        protected Vector3 followDirection;

        private float originalDrag;
        protected Rigidbody2D player;
        protected Enemy self;

        protected void Awake()
        {
            ai = GetComponent<AIPath>();
            self = GetComponent<Enemy>();
        }

        protected virtual void Start()
        {
            defaultSpeed = ai.maxSpeed;

            // Determine follow direction
            followDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            originalDrag = self.rb.linearDamping;

            //self.onStunned.AddListener(CancelDash);
            //self.onStunned.AddListener(DisableAttack);

            player = PlayerStatus.player.GetComponent<Rigidbody2D>();
        }

        protected virtual void FixedUpdate()
        {
            if (!PlayerStatus.IsAlive) return;

            // Get distance to player
            distanceToPlayer = Vector2.Distance(transform.position, player.position);

            switch (mode)
            {
                case AIMode.idle:
                    if (distanceToPlayer <= wakeDistance) switchState(AIMode.pathing);

                    Idle();
                    break;

                case AIMode.pathing:
                    if (distanceToPlayer <= rushDistance) switchState(AIMode.pushing);

                    Pathing();
                    break;

                case AIMode.pushing:
                    if (distanceToPlayer > rushDistance) switchState(AIMode.pathing);

                    Pushing();
                    /*if (self.canSeePlayer && canDash && distanceToPlayer >= minDashDistance && distanceToPlayer <= maxDashDistance)
                    {
                        if (!dashQueued)
                        {
                            Invoke("TryDashAttack", Random.Range(0, 2));
                            dashQueued = true;
                        }
                    }*/

                    ai.destination = player.position;
                    break;

                case AIMode.attacking:
                    // disable movement for a few ms
                    // attack
                    // check if player is near
                    //if (distance > rushDistance) switchState(AIMode.pathing);

                    Attacking();
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;

            // Stop moving
            if (dashing)
                AttackPlayer(collision, damage.damage * 1.5f);
            else
                switchState(AIMode.attacking);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;

            switchState(AIMode.pathing);
        }

        protected virtual void Idle()
        {
            ai.canMove = false;
        }

        protected virtual void Pathing()
        {
            ai.destination = playerPositionToFollow();
        }

        protected virtual void Pushing()
        {
            if (distanceToPlayer > rushDistance) switchState(AIMode.pathing);
        }

        protected virtual void Attacking()
        {
            if (canAttack) StartCoroutine(SwipeAttack());
        }

        protected virtual Vector3 playerPositionToFollow()
        {
            return player.position /*+ (followDirection * (player.velocity.magnitude / 2))*/;
        }

        private void TryDashAttack()
        {
            var val = Random.value;
            if (val > .9f) StartCoroutine(DashAttack());
            dashQueued = false;
        }

        private IEnumerator DashAttack()
        {
            if (attacking || !canDash) yield break;
            OnDashAttack?.Invoke();
            attacking = true;
            canDash = false;

            var player = PlayerStatus.player.transform;
            var dashDir = Vector2.one;
            ai.canMove = false;

            // Disable collision with enemies
            gameObject.layer = LayerMask.NameToLayer("WrittenWorksDash");

            float t = 0;
            while (t < 0.8f)
            {
                yield return new WaitForEndOfFrame();
                t += Time.deltaTime;
                dashDir = (player.position +
                           (Vector3)PlayerStatus.player.GetComponent<Rigidbody2D>().linearVelocity * 0.2f -
                           transform.position)
                    .normalized;
                transform.up = Vector3.Lerp(transform.up, dashDir, Mathf.Lerp(0.5f, 0, t / 0.8f));

                if (!self.canSeePlayer) CancelDash(0);
            }

            // target player for a few seconds

            print("locked on");


            // lock on
            dashing = true;
            yield return new WaitForSeconds(0.25f);
            print("dash");

            // Dash into player
            self.rb.linearDamping = originalDrag * 0.4f;
            self.rb.AddForce(transform.up * 25f, ForceMode2D.Impulse);

            OnDashStart?.Invoke();

            // Onhit


            yield return new WaitForSeconds(0.5f);
            self.rb.linearDamping = originalDrag;
            ai.canMove = true;
            attacking = false;
            dashing = false;

            // Enable collision with enemies
            gameObject.layer = LayerMask.NameToLayer("WrittenWorks");
            OnDashEnd?.Invoke();

            Invoke("EnableDash", Random.Range(minDashCooldown, maxDashCooldown));
        }

        private void CancelDash(float damage)
        {
            // Only allow cancel if still casting 
            if (dashing) return;
            CancelInvoke("EnableDash");
            print("canceld");

            attacking = false;
            ai.canMove = true;
            self.rb.linearDamping = originalDrag;
            gameObject.layer = LayerMask.NameToLayer("WrittenWorks");

            StopAllCoroutines();
            OnDashCancel?.Invoke();

            canDash = false;
            Invoke("EnableDash", 3);
        }

        private IEnumerator SwipeAttack()
        {
            OnSwipeAttack?.Invoke();
            ai.maxSpeed = defaultSpeed * 0.4f;
            yield return new WaitForSeconds(0.35f);

            var collision = Physics2D.OverlapCapsuleAll(transform.position + new Vector3(-0.01417112f, 0.5f),
                Vector2.one * 4 + new Vector2(1.168448f, 1.3f), CapsuleDirection2D.Horizontal,
                transform.rotation.eulerAngles.z);
            foreach (var col in collision)
                if (col.CompareTag("Player") && !attacking)
                {
                    attacking = true;

                    // check if player is still in range
                    var distance =
                        Vector3.Distance(transform.position,
                            col.transform.position); // TODO: use overlap to account for direction
                    var objectHit = Physics2D.Linecast(transform.position, col.transform.position, hitFilter.layerMask);
                    if (objectHit && objectHit.collider.CompareTag("Wall")) continue;

                    if (distance <= attackRange) AttackPlayer(col, damage.damage);


                    mode = AIMode.pathing;
                    attacking = false;
                }

            ai.maxSpeed = defaultSpeed;
        }

        protected virtual void AttackPlayer(Collider2D collision, float damage)
        {
            var playerRb = collision.GetComponent<Rigidbody2D>();

            // Give player knockback
            var kb = dashing ? knockbackAmount * 2.5f : knockbackAmount;
            playerRb.AddForce(transform.up.normalized * kb, ForceMode2D.Impulse);

            collision.GetComponent<PlayerStatus>().GiveDamage(damage, stunLength, DamageTypeFlag.Melee);

            // Knock self back
            self.rb.AddForce(-transform.up.normalized * (knockbackAmount / 3), ForceMode2D.Impulse);

            // Stop moving for awhile
            //self.Stun(reboundLength);
        }

        private void EnableDash()
        {
            canDash = true;
        }

        public void switchState(AIMode state)
        {
            //CancelInvoke();

            mode = state;
            if (mode == AIMode.pathing) ai.canMove = true;
        }

        public void switchState(int state)
        {
            //CancelInvoke();

            mode = (AIMode)state;
            if (mode == AIMode.pathing) ai.canMove = true;
        }

        private void DisableAttack(float t)
        {
            switchState(AIMode.pathing);

            canAttack = false;
            Invoke("EnableAttack", 1);
        }

        private void EnableAttack()
        {
            canAttack = true;
        }
    }
}