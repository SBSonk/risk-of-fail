using RiskOfFail.AI.Behaviors;

namespace RiskOfFail.Combat
{
    public class Quiz : Enemy
    {
        public override void Stun(float duration)
        {
            base.Stun(duration);

            // Reset shooting timer
            var shootingScript = attackScript as QuizAI;
            shootingScript.enableGunCooldown();
        }
    }
}