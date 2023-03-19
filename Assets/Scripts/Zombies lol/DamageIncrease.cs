using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIncrease : HealthIncrease
{
    protected override void Effect()
    {
        PlayerStatus.player.pShooting.damageMultiplier = 2;
    }
}