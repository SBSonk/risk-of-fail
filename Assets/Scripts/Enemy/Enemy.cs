using UnityEngine;
using UnityEngine.Events;
using Pathfinding;
using System.Collections;

public class Enemy : Alive
{
    public bool canSeePlayer = false;

    [Header("Enemy")]
    [SerializeField] EnemyType type = EnemyType.WrittenWorks;
    [SerializeField] int killScore;

    [Header("Health Bar")]
    [SerializeField] GameObject hBarPrefab;
    [SerializeField] Vector3 hbarOffset = new Vector3(0, 1.25f);

    [Header("Movement")]
    [SerializeField] float minSpeed = 10;
    [SerializeField] float maxSpeed = 15;
    [SerializeField] float speed = 10;
    [SerializeField] float hbarLerp = 0.5f, speedLerp = 0.125f;
    float spawnTime;

    public MonoBehaviour attackScript;
    public Rigidbody2D rb;

    GameObject healthParent;
    Transform healthBar;
    protected AIPath pathAI;
    protected bool afterStun;

    public UnityEvent<EnemyType> onEnemyDeath;

    protected void Start()
    {
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
    }

    private void Update()
    {
        canSeePlayer = Physics2D.Linecast(transform.position, PlayerStatus.player.transform.position).collider.CompareTag("Player");    

        // Make health bar follow enemy
        healthParent.transform.position = Vector3.Lerp(healthParent.transform.position, transform.position + hbarOffset, hbarLerp);

        // Healthbar animation  
        healthBar.transform.localScale = Vector3.Lerp(healthBar.transform.lossyScale, new Vector3(1 * (health / maxHealth), 1, 1), hbarLerp);

        // Reset speed after stun
        if (afterStun)
        {
            pathAI.maxSpeed = Mathf.Lerp(pathAI.maxSpeed, speed, Mathf.SmoothStep(0, 1, speedLerp));

            if (pathAI.maxSpeed >= 0.95f)
            {
                pathAI.maxSpeed = speed;
                afterStun = false;
            }
        }
    }

    IEnumerator HealthBarFade(float startOpacity, float finalOpacity, float t)
    {
        SpriteRenderer[] healthBarSprites = healthParent.GetComponentsInChildren<SpriteRenderer>();

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

    protected override void Death()
    {
        // Destroy health bar
        Destroy(healthParent);

        // Drop drops
        DropObject drop = GetComponent<DropObject>();
        if (drop) drop.startDrop();

        // Give player score
        float lifetime = Time.time - spawnTime;
        GameManager.GiveScore(killScore, lifetime);

        // Reduce alive enemies for the spawner
        onEnemyDeath?.Invoke(type);

        base.Death();
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

        Invoke("clearStun", duration);
    }

    protected virtual void clearStun()
    {
        pathAI.canMove = true;
        afterStun = true;
        stunned = false;
        attackScript.enabled = true;

        // Reset rigidbody velocities
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
    }
}
