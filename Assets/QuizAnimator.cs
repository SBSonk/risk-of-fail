using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class QuizAnimator : MonoBehaviour
{
    [SerializeField] Transform sprite;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ParticleSystem deathParticles;
    
    private Animator animator;
    Enemy enemy;
    AIPath pathing;
    Directions dirFacing = Directions.down;

    public bool canSwitchAnimations = true;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        pathing = GetComponent<AIPath>();
        
        enemy.onHit.AddListener(StunAnimation);
        enemy.onEnemyDeath.AddListener(DeathAnimation);
    }

    private void Update()
    {
        // Sprites
        sprite.position = transform.position + new Vector3(0, -0.75f);
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

    public void ShootAnimation()
    {
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
        Invoke("EnableAnimations", 0.5f);
    }
    
    public void DeathAnimation(EnemyType _)
    {
        deathParticles.transform.SetParent(null);
        Destroy(deathParticles.gameObject, 2f);
        deathParticles.Play();
    }
    
    void StunAnimation(float duration) 
    {
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
    
    void EnableAnimations() { canSwitchAnimations = true; }
    
    Directions VectorToDir(Vector2 input)
    {
        Directions final = dirFacing;

        if (input.x > 0.5f) final = Directions.right;
        else if (input.x < -0.5f) final = Directions.left;
        else if (input.y > 0.5f) final = Directions.up;
        else if (input.y < -0.5f) final = Directions.down;

        return final;
    }
}
