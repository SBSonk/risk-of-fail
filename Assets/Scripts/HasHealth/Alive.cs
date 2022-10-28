using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// This script is only meant to be inherited by other objects (players, anything that has health)
public class Alive : MonoBehaviour
{
    [Header("Stats")]
    public float health = 100;
    public float maxHealth = 100;
    public float damageCooldown = 0; // Dictates how long before you can take damage again
    public float damageReduction = 1;
    public float statusEffectTickrate = 1f; // How often a status effect ticks in seconds

    public List<StatusEffect> statuses;
    public GameObject damageIndicatorPrefab;

    public bool stunned;
    bool canBeDamaged = true;
    bool dead;

    public UnityEvent<float> onStunned;

    // Applies damage and returns damage taken
    public float GiveDamage(float amount, float stunLength, StatusEffect effect = null)
    {
        // Return if can't be damaged
        if (!canBeDamaged || dead) return 0;

        // Give effect, if any
        if (effect)
        {
            if (effect.inflictChance > Random.value)
            {
                statuses.Add(effect);
                InvokeRepeating("StatusEffectTick", statusEffectTickrate, statusEffectTickrate);
            }
        }

        // Calculate damage
        float damage = -amount / damageReduction;

        // Give damage
        if (health + damage <= 0) { OnDamage(damage); OnDeath(); return damage; }
        health += damage;

        // Apply stun
        Stun(stunLength);

        // Trigger animation, effects, etc
        OnDamage(damage);

        // Damage cooldown
        if (damageCooldown > 0)
        {
            canBeDamaged = false;
            Invoke("AllowDamage", damageCooldown);
        }

        return damage;
    }

    public virtual float GiveHealth(float amount)
    {
        // Give health
        health += amount;

        // Clamp health to normal values
        health = Mathf.Clamp(health, 0, maxHealth);

        OnDamage(amount);
        return amount;
    }

    public virtual void Stun(float duration) { return; }

    protected virtual void OnDamage(float damage) 
    {
        // Player damage indicator
        if (damage != 0)
        {
            // Offset position
            Vector3 pos = transform.position;
            pos += Vector3.up * Random.Range(1f, 3f);
            pos += Vector3.right * (Random.Range(-3f, 3f) * Mathf.PerlinNoise(transform.position.x * Time.time, transform.position.y * Time.time));

            // Spawn indicator
            DamageIndicator indicator = Instantiate(original: damageIndicatorPrefab,
                  position: pos, rotation: Quaternion.identity).GetComponent<DamageIndicator>();

            indicator.Initialize(damage);
        }
    }

    protected virtual void OnDeath() { dead = true; Destroy(this.gameObject); }

    // TODO: Make the status tick an event to subscribe in the game manager to optimize
    protected void StatusEffectTick()
    {
        // Stop repeating the status tick
        if (statuses.Count == 0)
        {
            CancelInvoke("StatusEffectTick"); 
            return;
        }

        foreach (StatusEffect s in statuses)
        {
            s.OnStatusTick(this);
        }
    }

    void AllowDamage()
    {
        canBeDamaged = true;
    }
}
