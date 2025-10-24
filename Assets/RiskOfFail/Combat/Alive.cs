using System;
using System.Collections.Generic;
using RiskOfFail.Combat.Enums;
using RiskOfFail.Combat.Interfaces;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace RiskOfFail.Combat
{
    // This script is only meant to be inherited by other objects (players, anything that has health)
    public abstract class Alive : MonoBehaviour
    {
        [Header("Stats")] public float health = 100;
        public float maxHealth = 100;
        
        public float damageReduction = 1;

        public HashSet<StatusEffect> activeStatusEffects = new HashSet<StatusEffect>();
        public GameObject damageIndicatorPrefab;
        public bool stunned;

        public UnityEvent<float, DamageTypeFlag> onHit, onHeal;
        public UnityEvent<DamageTypeFlag> onDeath;
        protected bool dead;

        private void OnEnable()
        {
            // Register Events
            foreach (var deathEvents in GetComponents<IOnDeath>())
            {
                onDeath.AddListener(deathEvents.OnDeath);
            }
        }

        private void OnDisable()
        {
            // Unsubscribe Events
            onDeath.RemoveAllListeners();
        }

        // Applies damage and returns damage taken
        public float GiveDamage(float amount, float stunLength, DamageTypeFlag damageFlag, StatusEffect effect = null)
        {
            // Return if can't be damaged
            if (dead) return 0;

            // Give effect, if any
            if (effect)
                if (effect.inflictChance > Random.value)
                {
                    activeStatusEffects.Add(effect);
                    InvokeRepeating("StatusEffectTick", 1, 1);
                }

            // Calculate damage
            var damage = -amount / damageReduction;

            // Give damage
            if (health + damage <= 0)
            {
                OnDamage(damage);
                Death(damageFlag);
                return damage;
            }

            health += damage;

            // Apply stun
            Stun(stunLength);

            // Trigger animation, effects, etc
            OnDamage(damage);

            onHit?.Invoke(damage, damageFlag);
            return damage;
        }

        public float GiveDamage(DamageSource damageSource, DamageTypeFlag flag, StatusEffect statusEffect = null)
        {
            return GiveDamage(damageSource.damage, damageSource.stunTime, flag, statusEffect);
        }

        public virtual float GiveHealth(float amount, DamageTypeFlag damageFlag)
        {
            // Give health
            health += amount;

            // Clamp health to normal values
            health = Mathf.Clamp(health, 0, maxHealth);

            OnDamage(amount);
            onHeal?.Invoke(health, damageFlag);
            return amount;
        }

        public virtual void Stun(float duration)
        {
            
        }

        protected virtual void OnDamage(float damage)
        {
            // Player damage indicator
            // Offset position
            var pos = transform.position;
            pos += Vector3.up * Random.Range(-2f, 2f);
            pos += Vector3.right * (Random.Range(-3f, 3f) *
                                    Mathf.PerlinNoise(transform.position.x * Time.time,
                                        transform.position.y * Time.time));

            // Spawn indicator
            var indicator = Instantiate(damageIndicatorPrefab,
                pos, Quaternion.identity).GetComponent<DamageIndicator>();

            indicator.Initialize(damage);
        }

        protected virtual void Death(DamageTypeFlag flag)
        {
            dead = true;
            onDeath?.Invoke(flag);

            Destroy(gameObject);
        }

        // TODO: Make the status tick an event to subscribe in the game manager to optimize
        protected void StatusEffectTick()
        {
            // Stop repeating the status tick
            if (activeStatusEffects.Count == 0)
            {
                CancelInvoke("StatusEffectTick");
                return;
            }

            foreach (var s in activeStatusEffects) s.OnStatusTick(this);
        }
    }
}