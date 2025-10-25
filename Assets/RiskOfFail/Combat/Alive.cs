using System;
using System.Collections.Generic;
using RiskOfFail.Combat.Classes;
using RiskOfFail.Combat.Enums;
using RiskOfFail.Combat.Interfaces;
using RiskOfFail.Combat.ScriptableObjects;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace RiskOfFail.Combat
{
    // This script is only meant to be inherited by other objects (players, anything that has health)
    public abstract class Alive : MonoBehaviour
    {
        [Header("Stats")] 
        public float maxHealth = 100;
        public float health { get; protected set; }
        
        public float damageReduction = 1;

        List<ActiveStatusEffect> activeStatusEffects = new List<ActiveStatusEffect>();

        [Header("Events")] 
        public UnityEvent<float, float, DamageTypeFlag> onHit;
        public UnityEvent<float, DamageTypeFlag> onHeal;
        public UnityEvent<DamageTypeFlag> onDeath;
        protected bool dead;

        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            Deactivate();
        }

        protected virtual void Initialize()
        {
            dead = false;
            
            health = maxHealth;
            
            // Register Events
            foreach (var deathEvents in GetComponents<IOnDeath>())
            {
                onDeath.AddListener(deathEvents.OnDeath);
            }

            foreach (var hitEvents in GetComponents<IOnHit>())
            {
                onHit.AddListener(hitEvents.OnHit);
            }
            
            foreach (var healEvents in GetComponents<IOnHeal>())
            {
                onHeal.AddListener(healEvents.OnHeal);
            }
            
            // Start status effect tick
            InvokeRepeating("StatusEffectTick", 1, 1);
        }

        protected virtual void Deactivate()
        {
            // Unsubscribe Events
            onDeath.RemoveAllListeners();
            onHit.RemoveAllListeners();
            onHeal.RemoveAllListeners();
            
            // Turn off status effect tick
            CancelInvoke("StatusEffectTick");
        }
        
        // Applies damage and returns damage taken
        public float GiveDamage(float amount, float stunTime, DamageTypeFlag damageFlag, StatusEffectBase effectBase = null)
        {
            if (dead) return 0;

            // Give effect, if any
            if (effectBase) TryApplyStatusEffect(effectBase);

            // Calculate damage
            var damage = -amount / damageReduction;

            // Give damage
            if (health + damage <= 0)
            {
                Death(damageFlag);
                return damage;
            }

            health += damage;

            onHit?.Invoke(damage, stunTime, damageFlag);
            return damage;
        }

        public float GiveDamage(DamageSource damageSource, DamageTypeFlag flag, StatusEffectBase statusEffectBase = null)
        {
            return GiveDamage(damageSource.damage, damageSource.stunTime, flag, statusEffectBase);
        }

        public float GiveHealth(float amount, DamageTypeFlag damageFlag)
        {
            // Give health
            health += amount;

            // Clamp health to normal values
            health = Mathf.Clamp(health, 0, maxHealth);

            onHeal?.Invoke(health, damageFlag);
            return amount;
        }

        void TryApplyStatusEffect(StatusEffectBase statusEffectBase)
        {
            if (statusEffectBase.inflictChance > Random.value)
            {
                activeStatusEffects.Add(new ActiveStatusEffect(statusEffectBase));
 
                statusEffectBase.OnApply(this);
            }
        }
        
        protected virtual void Death(DamageTypeFlag flag)
        {
            dead = true;
            onDeath?.Invoke(flag);

            Destroy(gameObject);
        }

        protected void StatusEffectTick()
        {
            for (int i = 0; i < activeStatusEffects.Count; i++)
            {
                activeStatusEffects[i].secondsLeft -= 1;

                activeStatusEffects[i].effect.OnStatusTick(this);

                if (activeStatusEffects[i].secondsLeft <= 0)
                {
                    activeStatusEffects[i].effect.OnClear(this);
                    activeStatusEffects.Remove(activeStatusEffects[i]);
                }
            }
        }
    }
}