using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageTrap : WalkableTrap
{
    public UnityEvent<Alive[]> OnTrapDamage;

    protected override void DoTrapDamage()
    {
        // Check if entities are still inside the trap
        for (int i = 0; i < entitiesInsideArea.Count; i++)
        {
            entitiesInsideArea[i].GiveDamage(dmg.damage, dmg.stunTime, null, dmg.useRawDamage);
        }
    }
}
