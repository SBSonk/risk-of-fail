using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadSpeedIncrease : HealthIncrease
{
    protected override void Effect()
    {
        PlayerStatus.player.pShooting.reloadMultiplier = 1.5f;
    }
}
