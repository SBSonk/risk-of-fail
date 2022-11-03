using UnityEngine;
using Pathfinding;
using System.Collections;
using UnityEngine.Events;

// I should really make this inherit QuizAI or something... too much work
public class WrittenWorksAttack : MonoBehaviour
{
    static int dashesQueued = 0;
    const int MAXDASHESQUEUED = 2;

    [SerializeField] float baseDamage, stunLength, knockbackAmount, reboundLength;
    [SerializeField] float rushDistance = 5f, attackRange = 5f;
    float defaultSpeed;

    public AIMode mode = AIMode.pathing;
    protected AIPath ai;
    protected Enemy self;
    protected Rigidbody2D player;

    protected Vector3 followDirection;

    float originalDrag;
    public float minDashDistance = 5f, maxDashDistance = 6f, minDashCooldown = 5f, maxDashCooldown = 10f;
    bool attacking, dashing, canDash = true;
    public UnityEvent OnSwipeAttack, OnDashAttack, OnDashStart, OnDashEnd, OnDashCancel;

    public Transform dashIndicator;

    float distanceToPlayer = 0;

    protected virtual void Start()
    {
        ai = GetComponent<AIPath>();
        self = GetComponent<Enemy>();
        player = GameObject.Find("Player").GetComponent<Rigidbody2D>();

        defaultSpeed = ai.maxSpeed;

        // Determine follow direction
        followDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        originalDrag = self.rb.drag;

        //self.onHit.AddListener(CancelDash);
    }

    protected virtual void FixedUpdate()
    {
        // Get distance to player
        distanceToPlayer = Vector2.Distance(transform.position, player.position);

        switch (mode)
        {
            case AIMode.pathing:
                if (distanceToPlayer <= rushDistance) switchState(AIMode.pushing);
        
                ai.destination = playerPositionToFollow();
                break;

            case AIMode.pushing:
                if (distanceToPlayer > rushDistance) switchState(AIMode.pathing);

                /*if (self.canSeePlayer && canDash && distanceToPlayer >= minDashDistance && distanceToPlayer <= maxDashDistance)
                {
                    StartCoroutine(DashAttack());
                }*/

                ai.destination = player.position;
                break;

            case AIMode.attacking:
                // disable movement for a few ms

                // attack
                // check if player is near
                StartCoroutine(SwipeAttack());
 

                //if (distance > rushDistance) switchState(AIMode.pathing);
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // Stop moving
        if (dashing)
        {
            AttackPlayer(collision);
        } else
        {
            mode = AIMode.attacking;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        mode = AIMode.pathing;
    }

    protected virtual Vector3 playerPositionToFollow()
    {
        return (Vector3)player.position + (followDirection * (player.velocity.magnitude / 2));
    }

    IEnumerator DashAttack()
    {
        if (attacking || !canDash || dashesQueued >= MAXDASHESQUEUED) yield break;
        dashesQueued++;
        OnDashAttack?.Invoke();
        print(dashesQueued);
        attacking = true;
        canDash = false;

        Transform player = PlayerStatus.player.transform;
        Vector2 dashDir = Vector2.one;
        ai.canMove = false;

        // Disable collision with enemies
        gameObject.layer = LayerMask.NameToLayer("WrittenWorksDash");

        float t = 0;
        while (t < 1.25f)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
            dashDir = ((player.position + ((Vector3) PlayerStatus.player.GetComponent<Rigidbody2D>().velocity * 0.2f)) - transform.position).normalized;
            transform.up = Vector3.Lerp(transform.up, dashDir, 0.5f);

            if (!self.canSeePlayer || distanceToPlayer > maxDashDistance) CancelDash(0);
        }

        // target player for a few seconds
     
        print("locked on");
        

        // lock on
        yield return new WaitForSeconds(0.25f);
        print("dash");

        // Dash into player
        self.rb.drag = originalDrag * 0.5f;
        self.rb.AddForce(dashDir * 25f, ForceMode2D.Impulse);

        OnDashStart?.Invoke();

        // Onhit
        dashing = true;


        yield return new WaitForSeconds(0.5f);
        self.rb.drag = originalDrag;
        ai.canMove = true;
        attacking = false;  
        dashing = false;

        // Enable collision with enemies
        gameObject.layer = LayerMask.NameToLayer("WrittenWorks");
        dashesQueued--;
        if (dashesQueued < 0) dashesQueued = 0;
        OnDashEnd?.Invoke();

        Invoke("EnableDash", Random.Range(minDashCooldown, maxDashCooldown));
    }

    void CancelDash(float damage)
    {
        // Only allow cancel if still casting 
        if (dashing || attacking) return;
        CancelInvoke();
        print("canceld");
        
        attacking = false;
        ai.canMove = true;
        self.rb.drag = originalDrag;
        gameObject.layer = LayerMask.NameToLayer("WrittenWorks");

        StopAllCoroutines();
        OnDashCancel?.Invoke();

        Invoke("EnableDash", 2);

        
        dashesQueued--;
        if (dashesQueued < 0) dashesQueued = 0;
    }

    IEnumerator SwipeAttack()
    {
        OnSwipeAttack?.Invoke();
        ai.maxSpeed = defaultSpeed * 0.4f;
        yield return new WaitForSeconds(0.35f);

        Collider2D[] collision = Physics2D.OverlapCapsuleAll(transform.position + new Vector3(-0.01417112f, 0.5f), Vector2.one * 4 + new Vector2(1.168448f, 1.3f), CapsuleDirection2D.Horizontal, transform.rotation.eulerAngles.z);
        foreach (Collider2D col in collision)
        {
            if (col.CompareTag("Player") && !attacking)
            {
                attacking = true;

                // check if player is still in range
                float distance = Vector3.Distance(transform.position, col.transform.position); // TODO: use overlap to account for direction
                if (distance <= attackRange)
                {
                    AttackPlayer(col);
                }

                mode = AIMode.pathing;
                attacking = false;
                
            }
        }

        ai.maxSpeed = defaultSpeed;
    }

    protected virtual void AttackPlayer(Collider2D collision)
    {
        Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();

        // Give player knockback
        float kb = dashing ? knockbackAmount * 2.5f : knockbackAmount;
        playerRb.AddForce(transform.up.normalized * kb, ForceMode2D.Impulse);

        collision.GetComponent<PlayerStatus>().GiveDamage(baseDamage, stunLength);

        // Knock self back
        self.rb.AddForce(-transform.up.normalized * (knockbackAmount / 3), ForceMode2D.Impulse);

        // Stop moving for awhile
        self.Stun(reboundLength);
    }

    void EnableDash()
    {
        canDash = true;
    }

    protected void switchState(AIMode state)
    {
        //CancelInvoke();

        mode = state;
    }
}
