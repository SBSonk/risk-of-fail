using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedIncrease : HealthIncrease
{
    protected override void Effect()
    {
        PlayerStatus.player.pMovement.moveSpeed = 8000;
        PlayerStatus.player.pMovement.baseSpeed = 8000;
    }
}
