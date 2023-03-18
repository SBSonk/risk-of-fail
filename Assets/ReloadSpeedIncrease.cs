using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadSpeedIncrease : MonoBehaviour
{
    public void Purchase()
    {
        PlayerStatus.player.pShooting.reloadMultiplier = 1.5f;
    }
}
