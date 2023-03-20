using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponBuy : Buyable
{
    public Weapon weapon;
    public SpriteRenderer weaponSprite;

    public UnityEvent CancelPurchase;
    
    private void Start()
    {
        weaponSprite.sprite = weapon.weaponSprite;
    }

    public void Purchase()
    {
        if (GameManager.CheckIfWeaponOwned(weapon))
        {
            CancelPurchase?.Invoke();
            return;
            // give ammo
        }
        GameManager.GiveWeaponZombies(weapon);
        PlayerStatus.player.pAnimations.ChangeWeaponSprite(PlayerStatus.player.pShooting.GetHeldWeapon());
        HudManager4.hud.UpdateWeaponIcon(PlayerStatus.player.pShooting.GetHeldWeapon());
    }

    protected override void PlayerInteract()
    {
        // Check for points
        if (GameManager.CheckIfWeaponOwned(weapon))
        {
            CancelPurchase?.Invoke();
            return;
            // give ammo
        }
        
        if (LevelStats.main.points >= price)
        {
            LevelStats.main.GiveScore(-price);
            OnPurchase?.Invoke();

            Purchase();
            if (!repeatable) interactable = false;
        }
    }
}
