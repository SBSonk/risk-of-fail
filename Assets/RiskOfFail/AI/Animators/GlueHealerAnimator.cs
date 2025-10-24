using Pathfinding;
using RiskOfFail.AI.Behaviors;
using UnityEngine;

namespace RiskOfFail.AI.Animators
{
    public class GlueHealerAnimator : QuizAnimator
    {
        private AIPath ai;
        private GlueHealer healer;
        private LineRenderer line;

        private void Start()
        {
            base.Start();

            healer = GetComponent<GlueHealer>();
            line = GetComponentInChildren<LineRenderer>();
            ai = GetComponent<AIPath>();
        }

        protected override void Update()
        {
            base.Update();
            if (healer.target && healer.target.health < healer.target.maxHealth && ai.reachedDestination)
            {
                line.enabled = true;
                line.SetPosition(0, transform.position);
                line.SetPosition(1, healer.target.transform.position);

                // Face heal dir
                dirFacing = VectorToDir(healer.target.transform.position - transform.position);
            }
            else
            {
                line.enabled = false;
            }
        }
    }
}