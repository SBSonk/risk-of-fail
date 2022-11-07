using UnityEngine;
using Pathfinding;

public class QuizAI : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] LayerMask los;
    [SerializeField] Gun weapon;

    [Header("Dodging")]
    [SerializeField] float dodgeChance = 25; // in percentages 25%
    [SerializeField] float dodgeCooldown = 0.75f;
    [SerializeField] float dodgeSpeed, dodgeDistance;
    [SerializeField] float enterShootRadius = 5f;
    [SerializeField] float timeToShoot = 0.25f, losWidth = 0.1f; // width for the raycasts
    bool canShoot = true, canDodge = true, gunCanShoot;

    [Header("Repositioning")]
    [SerializeField] float timeBeforePathfinding = 1; // Determine how long the quiz will wait before pursuing player again
    [SerializeField] float followDistanceMultiplier = 2; // How far the quiz will try to be from the player
    [SerializeField] int minShots, maxShots = 3;  // Shots to take before repositioning
    [SerializeField] float minReposRadius, maxReposRadius = 3;
    int shotsTaken, shotsToReposition;
    bool findNewPosition;

    AIPath ai;
    [SerializeField] Transform bulletSpawn;
    Rigidbody2D player, rb;
    public AIMode mode = AIMode.pathing;

    Vector3 followDirection;

    private void Start()
    {
        ai = GetComponent<AIPath>();
        player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();

        PlayerShooting.onPlayerShoot += tryDodge;

        // Determine follow direction
        followDirection = new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f)) * followDistanceMultiplier;
    }

    private void OnDestroy()
    {
        PlayerShooting.onPlayerShoot -= tryDodge;
    }

    private void FixedUpdate()
    {
        // Set destination to player
        if (mode != AIMode.repositioning)
            ai.destination = (Vector3) player.position + (followDirection * (player.velocity.magnitude / 2));

        // Get player distance
        float playerDistance = Vector3.Distance(transform.position, player.position);

        switch (mode)
        {
            case AIMode.pathing:
                // Check if ai is near the player
                if (playerDistance <= enterShootRadius && playerInLOS())
                {
                    switchState(AIMode.shooting);
                    break;
                }

                break;

            case AIMode.shooting:
                // Check if player left los to switch back to pathfinding mode
                if (!playerInLOS() || playerDistance >= enterShootRadius)
                {
                    Invoke("SwitchToPathing", timeBeforePathfinding);
                    break;
                }

                // Reposition every few shots of if player is too close
                if (shotsTaken < shotsToReposition || playerDistance < 4f)
                {
                    Shooting();
                }
                else switchState(AIMode.repositioning);
                break;

            case AIMode.repositioning:
                // Check if player left los to switch back to pathfinding mode
                if (!playerInLOS())
                {
                    Invoke("SwitchToPathing", timeBeforePathfinding);
                    break;
                }

                if (findNewPosition)
                {
                    findNewPosition = false;

                    // Look for a nearby place to reposition
                    Vector3 newPos = transform.position;
                    float distance = Random.Range(minReposRadius, maxReposRadius) * Random.Range(-1, 1);
                    newPos += transform.right * distance;

                    // Look for nearest node thats walkable
                    NNConstraint constraint = NNConstraint.Default;
                    constraint.walkable = true;
                    Vector3 node = AstarPath.active.GetNearest(newPos, constraint).position;

                    // Go to it
                    ai.destination = node;
                    ai.canMove = true;
                    break;
                }

                // Switch back to shooting
                if (ai.reachedDestination) switchState(AIMode.shooting);
                break;
        }
    }

    void Shooting()
    {
        // Check player position after 50ths of a second
        Vector3 predictedPlayerPos = player.position + (player.velocity / 3.5f);

        // Get distance from me to the player
        float distance = Vector3.Distance(bulletSpawn.position, predictedPlayerPos);

        // Get time to reach distance
        float travelTime = distance / (weapon.bulletVelocity * Time.fixedDeltaTime);

        predictedPlayerPos = (predictedPlayerPos - transform.position) * travelTime;
        transform.up = Vector3.Slerp(transform.up, predictedPlayerPos, .5f); // predict movement

        Debug.DrawLine(transform.position + transform.up, transform.position + (transform.up * 10), Color.red);

        if (gunCanShoot)
        {
            // Shoot at player
            if (canShoot)
            {
                weapon.ShootWeapon(bulletSpawn);
                shotsTaken++;

                // Apply firerate cooldown
                enableGunCooldown();
            }
        }
    }

    void tryDodge()
    {
        if (!canDodge) return;

        // Decide if i want to dodge
        if ((dodgeChance / 100f) < Random.value) return;

        // Cancel dodge if bullet isnt towards me
        GameObject incomingBullet = Gun.lastBulletShot;
        Vector3 target = transform.position - incomingBullet.transform.position;
        float angle = Vector2.Angle(target, incomingBullet.transform.right);
        if (!(angle < 15f)) return;

        // Check how long till the bullet hits me
        float distance = Vector3.Distance(transform.position, incomingBullet.transform.position);
        float travelTime = (distance / (incomingBullet.GetComponent<Rigidbody2D>().velocity.magnitude));

        // Invoke dodge .1s before bullet hits me
        Invoke("Dodge", (float) System.Math.Round(travelTime - 0.1f, 4));

    }

    void Dodge()
    {
        // Check if left or right is clear
        RaycastHit2D left = Physics2D.Raycast(transform.position, -transform.right, dodgeDistance, los);
        RaycastHit2D right = Physics2D.Raycast(transform.position, transform.right, dodgeDistance, los);

        if (!left && !right)
        {
            // Decide random direction to dodge
            float dir = Mathf.Sign(Random.Range(-1, 1));

            rb.AddForce(transform.right * dir * dodgeSpeed, ForceMode2D.Impulse);
        }
        else if (!left)
        {
            rb.AddForce(-transform.right * dodgeSpeed, ForceMode2D.Impulse);
        }
        else if (!right)
        {
            rb.AddForce(transform.right * dodgeSpeed, ForceMode2D.Impulse);
        }
        else
        {
            return; // Cancel dodge
        }

        // Dodge cooldown
        canDodge = false;
        Invoke("EnableDodge", dodgeCooldown);
    }

    public void enableGunCooldown()
    {
        // Reset invokes
        CancelInvoke("EnableGun");

        gunCanShoot = false;
        Invoke("EnableGun", weapon.fireRate);
    }

    void switchState(AIMode state)
    {
        CancelInvoke();
        mode = state;

        // Reset shots and set new value
        shotsTaken = 0;
        shotsToReposition = Random.Range(minShots, maxShots);


        // Disable shooting immediately after switching to shoot mode
        if (state == AIMode.shooting)
        {
            ai.canMove = false;
            gunCanShoot = true;
            canShoot = false;

            // Randomize time to shoot
            Invoke("EnableShooting", timeToShoot * Random.Range(.25f, 1.5f));
        }

        if (state == AIMode.pathing) ai.canMove = true;

        if (state == AIMode.repositioning) findNewPosition = true;

        // Reset dodge cooldown when switching states
        Invoke("EnableDodge", dodgeCooldown);
    }

    // Returns true if theres a straight line between me and the player
    bool playerInLOS()
    {
        //RaycastHit2D left = Physics2D.Linecast(transform.position + (transform.right * losWidth), player.position + (Vector2)(-player.transform.right * losWidth), los);
        RaycastHit2D center = Physics2D.Linecast(transform.position, player.position, los);
        //RaycastHit2D right = Physics2D.Linecast(transform.position + (-transform.right * losWidth), player.position + (Vector2)(player.transform.right * losWidth), los);

        print(center.collider.name);
        return center.collider.CompareTag("Player");
    }

    void SwitchToPathing() { switchState(AIMode.pathing); }

    void EnableShooting() { canShoot = true; }

    void EnableDodge() { canDodge = true; }

    void EnableGun() { gunCanShoot = true; }
}
