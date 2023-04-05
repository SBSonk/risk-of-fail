using System;
using System.Collections;
using System.Collections.Generic;
using FirstGearGames.SmoothCameraShaker;
using UnityEngine;
using UnityEngine.Events;

public class DamageTrap : WalkableTrap
{
    [SerializeField] protected DamageSource dmg;
    [SerializeField] protected float timeTillDamage;
    [SerializeField] protected float speedMultiplier = 1;
    
    public ShakeData triggerShake;
    public string animationName;
    public Animator animator;
    public Collider2D collider;
    
    public UnityEvent<List<Alive>> OnTrapDamage;
    public UnityEvent TrapDamage;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        
        Invoke("DoTrapDamage", timeTillDamage / speedMultiplier);
    }

    public void PlayAnimation()
    {
        animator.speed = speedMultiplier;
        animator.Play(animationName);
    }
    
    protected override void DoTrapDamage()
    {
        // Check if entities are still inside the trap
        for (int i = 0; i < entitiesInsideArea.Count; i++)
        {
            entitiesInsideArea[i].GiveDamage(dmg.damage, dmg.stunTime, KillFlag.AreaOfEffect, null, dmg.useRawDamage);
        }
        
        TrapDamage?.Invoke();
        OnTrapDamage?.Invoke(entitiesInsideArea);
        
        if (damageSound) damageSound.PlaySFX();
        if (triggerShake) CameraShakerHandler.Shake(triggerShake);
        
        AstarPath.active.UpdateGraphs(collider.bounds, 0.5f);  
    }
}    
