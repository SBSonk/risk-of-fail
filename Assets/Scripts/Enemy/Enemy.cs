using UnityEngine;
using UnityEngine.Events;
using Pathfinding;
using System.Collections;

public class Enemy : Alive
{
    public static float globalEnemyHealthScale = 1;
    
    public bool canSeePlayer = false;

    [Header("Enemy")]
    [SerializeField] protected EnemyType type = EnemyType.WrittenWorks;
    [SerializeField] protected int killScore;

    [Header("Health Bar")]
    [SerializeField] GameObject hBarPrefab;
    [SerializeField] Vector3 hbarOffset = new Vector3(0, 1.25f);

    [Header("Movement")]
    [SerializeField] float minSpeed = 10;
    [SerializeField] float maxSpeed = 15;
    [SerializeField] float speed = 10;
    [SerializeField] float hbarLerp = 0.5f, StunRecoverTime = 0.125f;
    protected float spawnTime;

    public MonoBehaviour attackScript;
    public Rigidbody2D rb;

    protected GameObject healthParent;
    Transform healthBar;
    protected AIPath pathAI;

    public UnityEvent<EnemyType> onEnemyDeath;

    SpriteRenderer[] healthBarSprites;

    protected void Start()
    {
        health *= globalEnemyHealthScale;
        maxHealth *= globalEnemyHealthScale;
        
        // Initialize AI
        pathAI = GetComponent<AIPath>();
        healthParent = Instantiate(hBarPrefab);
        healthBar = healthParent.transform.GetChild(0);
        rb = GetComponent<Rigidbody2D>();

        // Randomize speed
        speed = Random.Range(minSpeed, maxSpeed);
        pathAI.maxSpeed = speed;

        // Keep track of lifetime
        spawnTime = Time.time;

        healthBarSprites = healthParent.GetComponentsInChildren<SpriteRenderer>();
        attackScript.enabled = true;
    }

    private void Update()
    {
        if (!PlayerStatus.IsAlive) return;

        // Check if there is environment collision in the way
        canSeePlayer = !Physics2D.Linecast(transform.position, PlayerStatus.player.transform.position, LayerMask.NameToLayer("Environment"));    

        // Make health bar follow enemy
        if (healthParent)
        {
            healthParent.transform.position = Vector3.Lerp(healthParent.transform.position, transform.position + hbarOffset, hbarLerp);

            // Healthbar animation  
            healthBar.transform.localScale = Vector3.Lerp(healthBar.transform.lossyScale, new Vector3(1 * (health / maxHealth), 1, 1), hbarLerp);
        }
    }

    IEnumerator StunRecover(float time)
    {
        float startSpeed = pathAI.maxSpeed;

        float t = 0;
        while (t < time)
        {
            pathAI.maxSpeed = Mathf.Lerp(startSpeed, speed, t / time);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        pathAI.maxSpeed = speed;
    }

    IEnumerator HealthBarFade(float startOpacity, float finalOpacity, float t)
    {
        float opacity = startOpacity;
        float time = 0;
        while (time < t)
        {
            opacity = Mathf.Lerp(opacity, finalOpacity, time / t);
            
            for (int i = 0; i < healthBarSprites.Length; i++)
            {
                Color c = healthBarSprites[i].color;
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

        CancelInvoke("HideHealthBar");
        healthParent.SetActive(true);
        StartCoroutine(HealthBarFade(0, 1, 0.3f));
        Invoke("HideHealthBar", 2.5f);

        onHit?.Invoke(damage);
    }

    void HideHealthBar()
    {
        StartCoroutine(HealthBarFade(1, 0, 0.3f));
    }

    protected override void Death(KillFlag flag)
    {
        // Destroy health bar
        Destroy(healthParent);

        // Drop drops
        DropObject drop = GetComponent<DropObject>();
        if (drop) drop.startDrop();

        // Give player score
        float lifetime = Time.time - spawnTime;
        LevelStats.main.GiveScore(killScore, lifetime, flag);

        // Reduce alive enemies for the spawner
        onEnemyDeath?.Invoke(type);

        LevelStats.main.EnemyKilled(flag); // Should use an event probably

        base.Death(flag);
    }

    public override void Stun(float duration)
    {
        // Can't get stunned twice
        if (stunned) return;

        onStunned?.Invoke(duration);
        stunned = true;
        pathAI.canMove = false;
        pathAI.maxSpeed = 0;
        attackScript.enabled = false;

        StartCoroutine(clearStun(duration));
    }

    protected IEnumerator clearStun(float time)
    {
        yield return new WaitForSeconds(time);
        
        pathAI.canMove = true;
        stunned = false;
        attackScript.enabled = true;

        // Reset rigidbody velocities
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;

        StartCoroutine(StunRecover(StunRecoverTime));
    }
}
