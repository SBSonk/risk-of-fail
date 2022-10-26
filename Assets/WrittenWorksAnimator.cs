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

    private void Awake()
    {
        pathing = GetComponent<AIPath>();
        spriteRenderer = sprite.GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        spriteRenderer.flipX = dirFacing == Directions.left;
        dirFacing = VectorToDir((pathing.destination - transform.position).normalized);

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

}
