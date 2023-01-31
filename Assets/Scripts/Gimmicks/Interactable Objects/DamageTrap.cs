using System;
using System.Collections;
using System.Collections.Generic;
using FirstGearGames.SmoothCameraShaker;
using UnityEngine;
using UnityEngine.Events;

public class DamageTrap : WalkableTrap
{
    public ShakeData triggerShake;
    public string animationName;
    public Animator animator;
    
    public UnityEvent<List<Alive>> OnTrapDamage;
    public UnityEvent TrapDamage;

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
            entitiesInsideArea[i].GiveDamage(dmg.damage, dmg.stunTime, null, dmg.useRawDamage);
        }
        
        TrapDamage?.Invoke();
        OnTrapDamage?.Invoke(entitiesInsideArea);
        
        if (damageSound) damageSound.PlaySFX();
        if (triggerShake) CameraShakerHandler.Shake(triggerShake);
    }
}    
