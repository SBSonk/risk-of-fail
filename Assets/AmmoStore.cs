using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoStore : MonoBehaviour
{
    public void Purchase()
    {
        PlayerStatus.player.pShooting.GetHeldWeapon().pool += 100;
    }
}
