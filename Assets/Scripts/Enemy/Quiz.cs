using UnityEngine;

public class Quiz : Enemy
{
    public override void Stun(float duration)
    {
        base.Stun(duration);

        // Reset shooting timer
        QuizAI shootingScript = attackScript as QuizAI;
        shootingScript.enableGunCooldown();
    }
}
