using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIncrease : MonoBehaviour
{
    public void Purchase()
    {
        PlayerStatus.player.pShooting.damageMultiplier = 2;
    }
}