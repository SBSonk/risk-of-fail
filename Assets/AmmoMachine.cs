using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoMachine : Buyable
{
    protected override void PlayerInteract()
    {
        // Check for points
        if (LevelStats.main.points >= price && PlayerStatus.player.pShooting.GetHeldWeapon().weapon is Gun)
        {
            PlayerStatus.player.pShooting.GiveAmmo(PlayerStatus.player.pShooting.GetHeldWeapon(),
                Mathf.RoundToInt(PlayerStatus.player.pShooting.GetHeldWeapon().weapon.defaultAmmoCount / 1.5f));
            LevelStats.main.GiveScore(-price);
            OnPurchase?.Invoke();

            if (!repeatable)
            {
                interactable = false;
                interactIcon.enabled = false;
            }
        }
    }
}
