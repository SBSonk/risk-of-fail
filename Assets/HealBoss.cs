using System;
using System.Collections;
using System.Collections.Generic;
using RiskOfFail.Combat;
using UnityEngine;
using UnityEngine.Events;

public class HealBoss : MonoBehaviour
{
    public Alive self;
    public float healAmount = 5;
    public float healInterval = 5;

    public bool canHeal = true;

    public UnityEvent OnHealTarget, OnHealDisabled, OnHealEnabled;

    public void StartLoop(Alive target, float delay = 0)
    {
        StartCoroutine(HealLoop(target, delay));
    }

    public void DisableHeal()
    {
        self.immune = true;
        canHeal = false;
        OnHealDisabled?.Invoke();
    }

    public void EnableHeal()
    {
        self.immune = false;
        canHeal = true;
        OnHealEnabled?.Invoke();
    }

    IEnumerator HealLoop(Alive target, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        while (true)
        {
            if (!target) break;
            
            if (canHeal) target.GiveHealth(healAmount);
            OnHealTarget?.Invoke();

            yield return new WaitForSeconds(healInterval);
        }
    }
}
