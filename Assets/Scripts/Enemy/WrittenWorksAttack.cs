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
                // Check if near push threshhold       
                if (distance <= rushDistance) switchState(AIMode.pushing);
        
                // Pathfind to where the player will be
                ai.destination = playerPositionToFollow();

                break;

            case AIMode.pushing:
                // Check if the player is far
                if (distance > rushDistance) switchState(AIMode.pathing);

                // Pathfind to exactly where the player is
                ai.destination = player.position;

                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Return if the player isnt the one colliding
        if (!collision.CompareTag("Player")) return;

        AttackPlayer(collision);
    }

    // Choose where to target player
    protected virtual Vector3 playerPositionToFollow()
    {
        return (Vector3)player.position + (followDirection * (player.velocity.magnitude / 2));
    }

    // Attacks player
    protected virtual void AttackPlayer(Collider2D collision)
    {
        Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();

        // Give player knockback
        playerRb.AddForce(transform.up.normalized * knockbackAmount, ForceMode2D.Impulse);

        // Damage and Stun player
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
