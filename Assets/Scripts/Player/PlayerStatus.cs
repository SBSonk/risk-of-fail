using System.Collections;
using UnityEngine;

public class PlayerStatus : Alive
{
    public static PlayerStatus player;

    public PlayerMovement pMovement;
    public PlayerShooting pShooting;
    public PlayerAnimations pAnimations;

    public float defaultSpeed = 10;

    public delegate void OnPlayerDamage();
    public static event OnPlayerDamage onPlayerDamage;

    public delegate void OnPlayerHeal();
    public static event OnPlayerDamage onPlayerHeal;

    private void Awake()
    {
        player = this;

        pMovement = GetComponent<PlayerMovement>();
        pShooting = GetComponent<PlayerShooting>();
        pAnimations = GetComponent<PlayerAnimations>();
        pMovement.moveSpeed = defaultSpeed;
    }

    protected override void OnDamage(float damage)
    {
        // Play indicators
        base.OnDamage(damage);

        if (Mathf.Sign(damage) == 1) return;

        if (onPlayerDamage != null)
            onPlayerDamage.Invoke();
    }

    protected override void Death()
    {
        dead = true;

        pMovement.enabled = false;
        pShooting.enabled = false;
        pAnimations.enabled = false;

        onDeath?.Invoke();
    }

    public override float GiveHealth(float amount)
    {
        if (onPlayerHeal != null) onPlayerHeal.Invoke();
        return base.GiveHealth(amount);
    }

    public override void Stun(float duration)
    {
        StartCoroutine(TakeStun(duration));
    }

    IEnumerator TakeStun(float duration)
    {
        pMovement.moveSpeed = defaultSpeed / 2;
        pMovement.removeDodge(duration);

        // Disable switching animations
        pAnimations.canSwitchAnimation = false;

        yield return new WaitForSeconds(duration);

        pMovement.moveSpeed = defaultSpeed;

        // Reenable animation switching
        pAnimations.canSwitchAnimation = true;

    }
}
