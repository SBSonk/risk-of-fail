using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class WrittenWorksAnimator : MonoBehaviour
{
    [SerializeField] Transform sprite;
    [SerializeField] Animator animator;
    SpriteRenderer spriteRenderer;
    Directions dirFacing = Directions.down;
    AIPath pathing;
    Enemy enemy;
    [SerializeField] ParticleSystem deathParticles;

    public bool canSwitchAnimations = true;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        pathing = GetComponent<AIPath>();
        spriteRenderer = sprite.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        enemy.onStunned.AddListener(StunAnimation);
    }

    private void Update()
    {
        if (!canSwitchAnimations) return;
        if (spriteRenderer != null)
        dirFacing = VectorToDir((pathing.destination - transform.position).normalized);
        spriteRenderer.flipX = dirFacing == Directions.left;


        sprite.rotation = Quaternion.identity;

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

        canSwitchAnimations = false; 
        Invoke("EnableAnimations", duration + 0.5f); 
    }

    void EnableAnimations() { canSwitchAnimations = true; }

    public void DeathAnimation()
    {
        deathParticles.transform.SetParent(null);
        Destroy(deathParticles.gameObject, 2f);
        deathParticles.Play();
    }
}
