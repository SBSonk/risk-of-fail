using UnityEngine;
using Pathfinding;

// I should really make this inherit QuizAI or something... too much work
public class WrittenWorksAttack : MonoBehaviour
{
    [SerializeField] float baseDamage, stunLength, knockbackAmount, reboundLength;
    [SerializeField] float rushDistance = 5f;

    public AIMode mode = AIMode.pathing;
    protected AIPath ai;
    protected Enemy self;
    protected Rigidbody2D player;

    protected Vector3 followDirection;

    protected virtual void Start()
    {
        ai = GetComponent<AIPath>();
        self = GetComponent<Enemy>();
        player = GameObject.Find("Player").GetComponent<Rigidbody2D>();

        // Determine follow direction
        followDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }

    protected virtual void FixedUpdate()
    {
        // Get distance to player
        float distance = Vector2.Distance(transform.position, player.position);

        switch (mode)
        {
            case AIMode.pathing:
                if (distance <= rushDistance) switchState(AIMode.pushing);
        
                ai.destination = playerPositionToFollow();
                break;

            case AIMode.pushing:
                if (distance > rushDistance) switchState(AIMode.pathing);

                ai.destination = player.position;
                break;

            case AIMode.attacking:
                // disable movement for a few ms

                // attack

                mode = AIMode.pathing;
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // Stop moving

        mode = AIMode.attacking;
        AttackPlayer(collision);
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

    protected virtual void AttackPlayer(Collider2D collision)
    {
        Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();

        // Give player knockback
        playerRb.AddForce(transform.up.normalized * knockbackAmount, ForceMode2D.Impulse);

        collision.GetComponent<PlayerStatus>().GiveDamage(baseDamage, stunLength);

        // Knock self back
        self.rb.AddForce(-transform.up.normalized * (knockbackAmount / 2), ForceMode2D.Impulse);

        // Stop moving for awhile
        self.Stun(reboundLength);
    }

    protected void switchState(AIMode state)
    {
        CancelInvoke();

        mode = state;
    }
}
