using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QAAnimator : QuizAnimator
{
    private QuarterlyAssessmentAttack attack;

    protected override void Start()
    {
        base.Start();

        attack = GetComponent<QuarterlyAssessmentAttack>();
    }

    protected override void WalkAnimation()
    {
        if (attack.mode == AIMode.pushing)
        {
            switch (dirFacing)
            {
                case Directions.up:
                    animator.Play("rush_u");
                    break;

                case Directions.right:
                    animator.Play("rush_r");
                    break;

                case Directions.down:
                    animator.Play("rush_d");
                    break;

                case Directions.left:
                    animator.Play("rush_l");
                    break;
            }
        }
        else
        {
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
                    animator.Play("walk_l");
                    break;
            }
        }

    }

    public override void ShootAnimation()
    {
        switch (dirFacing)
        {
            case Directions.up:
                animator.Play("explode_u");
                break;

            case Directions.right:
                animator.Play("explode_r");
                break;

            case Directions.down:
                animator.Play("explode_d");
                break;

            case Directions.left:
                animator.Play("explode_l");
                break;
        }

        CancelInvoke("EnableAnimations");
        canSwitchAnimations = false;
    }
}
