using UnityEngine;

public class Quiz : Enemy
{
    public override void Stun(float duration)
    {
        // Can't get stunned twice
        if (stunned) return;

        stunned = true;
        pathAI.maxSpeed = 0;
        attackScript.enabled = false;

        Invoke("clearStun", duration);

        // Reset shooting timer
        QuizAI shootingScript = attackScript as QuizAI;
        shootingScript.enableGunCooldown();
    }

    protected override void clearStun()
    {
        afterStun = true;
        stunned = false;
        attackScript.enabled = true;

        // Reset rigidbody velocities
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
    }
}
