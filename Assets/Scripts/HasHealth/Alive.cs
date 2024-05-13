using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

// This script is only meant to be inherited by other objects (players, anything that has health)
public abstract class Alive : MonoBehaviour
{
    [Header("Stats")]
    public float health = 100;
    public float maxHealth = 100;
    public bool immune = false;
    public float damageCooldown = 0; // Dictates how long before you can take damage again
    public float damageReduction = 1;
    public float statusEffectTickrate = 1f; // How often a status effect ticks in seconds

    public List<StatusEffect> statuses;
    public GameObject damageIndicatorPrefab;

    public Rigidbody2D deathFlingPrefab;

    public bool stunned;
    protected bool canBeDamaged = true;
    protected bool dead;

    public UnityEvent<float> onHit, onStunned, onHeal;
    public UnityEvent<KillFlag> onDeath;
    public AudioSource deathSound;
    
    // Applies damage and returns damage taken
    public float GiveDamage(float amount, float stunLength, KillFlag flag, StatusEffect effect = null, bool giveRawDamage = false)
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
        float damage = giveRawDamage ? -amount : -amount / damageReduction;

        if (immune) damage = 0;
        
        // Give damage
        if (health + damage <= 0) 
        { 
            OnDamage(damage); 
            Death(flag); 
            return damage; 
        }

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

        onHit?.Invoke(damage);
        return damage;
    }

    public float GiveDamage(DamageSource damageSource, KillFlag flag, StatusEffect statusEffect = null)
    {
        return GiveDamage(damageSource.damage, damageSource.stunTime, flag, statusEffect, damageSource.useRawDamage);
    }

    public virtual float GiveHealth(float amount)
    {
        // Give health
        health += amount;

        // Clamp health to normal values
        health = Mathf.Clamp(health, 0, maxHealth);

        OnDamage(amount);
        onHeal?.Invoke(health);
        return amount;
    }

    public virtual void Stun(float duration) { return; }

    protected virtual void OnDamage(float damage) 
    {
        // Player damage indicator
         // Offset position
        Vector3 pos = transform.position;
        pos += Vector3.up * Random.Range(-2f, 2f);
        pos += Vector3.right * (Random.Range(-3f, 3f) * Mathf.PerlinNoise(transform.position.x * Time.time, transform.position.y * Time.time));

        // Spawn indicator
        DamageIndicator indicator = Instantiate(original: damageIndicatorPrefab,
        position: pos, rotation: Quaternion.identity    ).GetComponent<DamageIndicator>();

        indicator.Initialize(damage);
    }

    protected virtual void Death(KillFlag flag) 
    {
        dead = true; 
        onDeath?.Invoke(flag);

        if (deathFlingPrefab)
        {
            var rb = Instantiate(deathFlingPrefab, transform.position, quaternion.identity);
            
            rb.AddForce(new Vector3(Random.Range(1, -1f) * Random.Range(5, 10f),  Random.Range(2.5f, 10f)), ForceMode2D.Impulse);
            rb.AddTorque(-Mathf.Sign(rb.velocity.x) * Random.Range(5, 10f), ForceMode2D.Impulse);
        }
        
        if (deathSound) Instantiate(deathSound, transform.position, transform.rotation);
        Destroy(gameObject); 
    }

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
