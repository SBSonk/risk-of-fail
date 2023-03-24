using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;

public class WrittenWorksAnimator : MonoBehaviour
{
    [SerializeField] Transform sprite;
    [SerializeField] Animator animator;
    SpriteRenderer spriteRenderer;
    Directions dirFacing = Directions.down;
    AIPath pathing;
    Enemy enemy;
    WrittenWorksAttack enemyAttack;
    [SerializeField] ParticleSystem deathParticles, dashParticles;

    [SerializeField] SpriteRenderer dashIndicator;
    [SerializeField] SpriteRenderer[] arrowIndicator;

    public bool canSwitchAnimations = true;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemyAttack = GetComponent<WrittenWorksAttack>();
        pathing = GetComponent<AIPath>();
        spriteRenderer = sprite.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        enemy.onHit.AddListener(StunAnimation);
        enemyAttack.OnSwipeAttack.AddListener(SwipeAnimation);
        enemy.onEnemyDeath.AddListener(DeathAnimation);
        //enemyAttack.OnDashAttack.AddListener(DashAnimation);
        //enemyAttack.OnDashStart.AddListener(DashStart);
        //enemyAttack.OnDashEnd.AddListener(DashFinish);
        //enemyAttack.OnDashCancel.AddListener(DashCancel);
    }

    private void Update()
    {
        dashIndicator.transform.position = transform.position + (Vector3.up * 1.5f);
        dashIndicator.transform.rotation = Quaternion.identity;


        // Sprites
        sprite.rotation = Quaternion.identity;

        if (!canSwitchAnimations) return;

        if (enemy.canSeePlayer)
        {
            dirFacing = VectorToDir((pathing.destination - transform.position).normalized);
        } else
        {
            dirFacing = VectorToDir(pathing.velocity.normalized);
        }
        
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

    Directions VectorToDir(Vector2 input)
    {
        Directions final = dirFacing;

        if (input.x > 0.5f) final = Directions.right;
        else if (input.x < -0.5f) final = Directions.left;
        else if (input.y > 0.5f) final = Directions.up;
        else if (input.y < -0.5f) final = Directions.down;

        return final;
    }

    void StunAnimation(float duration) 
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

    void SwipeAnimation()
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

    void DashAnimation()
    {
        StartCoroutine(SprFunctions.Fade(dashIndicator, Color.clear, Color.white, 0.1f));

        StartCoroutine(SprFunctions.Fade(arrowIndicator[0], Color.clear, Color.white, 0.1f));;
        StartCoroutine(SprFunctions.Fade(arrowIndicator[1], Color.clear, Color.white, 1.05f));

        dashParticles.Play();
    }

    void DashStart()
    {
        StartCoroutine(SprFunctions.Fade(dashIndicator, Color.white, Color.clear, 0.1f));
        foreach (SpriteRenderer s in arrowIndicator)
        {
            StartCoroutine(SprFunctions.Fade(s, Color.white, Color.clear, 0.1f));
        }
    }

    void DashFinish()
    {
        dashParticles.Stop();
    }

    void DashCancel()
    {
        StopAllCoroutines();

        if (dashIndicator.color != Color.clear)
        {
            StartCoroutine(SprFunctions.Fade(dashIndicator, Color.white, Color.clear, 0.1f));
            foreach (SpriteRenderer s in arrowIndicator)
            {
                StartCoroutine(SprFunctions.Fade(s, Color.white, Color.clear, 0.1f));
            }
        }

        dashParticles.Clear();
        dashParticles.Stop();

        canSwitchAnimations = true;
    }

    void EnableAnimations() { canSwitchAnimations = true; }

    public void DeathAnimation(EnemyType _)
    {
        deathParticles.transform.SetParent(null);
        Destroy(deathParticles.gameObject, 2f);
        deathParticles.Play();
    }
}
