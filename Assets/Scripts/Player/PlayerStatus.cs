using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatus : Alive
{
    public static PlayerStatus player;

    public PlayerMovement pMovement;
    public PlayerShooting pShooting;
    public PlayerAnimations pAnimations;
    public PlayerSFXManager pSFXManager;

    public float defaultSpeed = 10;

    public UnityEvent OnPlayerDamage, OnPlayerHeal;

    private void Awake()
    {
        player = this;

        pMovement = GetComponent<PlayerMovement>();
        pShooting = GetComponent<PlayerShooting>();
        pAnimations = GetComponent<PlayerAnimations>();
        pSFXManager = GetComponent<PlayerSFXManager>();

        pMovement.moveSpeed = defaultSpeed;

        pShooting.Initialize();
        pAnimations.Initialize(pShooting, pMovement, this);
        pSFXManager.Initialize(pShooting, pMovement);

    }

    protected override void OnDamage(float damage)
    { 
        // Play indicators
        base.OnDamage(damage);

        if (Mathf.Sign(damage) == 1) return;

        OnPlayerDamage?.Invoke();
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
        OnPlayerHeal?.Invoke();
        return base.GiveHealth(amount);
    }

    public override void Stun(float duration)
    {
        StartCoroutine(TakeStun(duration));
    }

    IEnumerator TakeStun(float duration)
    {
        pMovement.moveSpeed = defaultSpeed / 2;

        // Disable switching animations
        pAnimations.canSwitchAnimation = false;

        yield return new WaitForSeconds(duration);

        pMovement.moveSpeed = defaultSpeed;

        // Reenable animation switching
        pAnimations.canSwitchAnimation = true;

    }
}
