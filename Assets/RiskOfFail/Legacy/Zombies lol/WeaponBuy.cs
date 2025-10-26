using RiskOfFail.Combat;
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
        PlayerShooting shooting = PlayerStatus.instance.shooting;
        if (shooting.CheckIfWeaponOwned(weapon))
        {
            if (weapon is Gun)
                PlayerStatus.instance.shooting.GiveAmmo(PlayerStatus.instance.shooting.GetWeaponFromInventory(weapon),
                    PlayerStatus.instance.shooting.GetHeldWeapon().weapon.defaultAmmoCount);

            return;
            // give ammo
        }

        if (!shooting.CheckIfWeaponOwned(weapon))
        {
            var newWeapon = new InventoryWeapon(weapon, weapon.clipSize, weapon.defaultAmmoCount);
            if (shooting.InventoryFull()) shooting.ReplaceWeapon(newWeapon, shooting.GetHeldIndex());
            else shooting.GiveWeapon(newWeapon);
        }
        else
        {
            shooting.GiveAmmo(shooting.GetWeaponFromInventory(weapon), Mathf.FloorToInt(weapon.defaultAmmoCount / 4f));
        }

        PlayerStatus.instance.animations.ChangeWeaponSprite(PlayerStatus.instance.shooting.GetHeldWeapon());
        HudManager4.hud.UpdateWeaponIcon(PlayerStatus.instance.shooting.GetHeldWeapon());
    }

    protected override void PlayerInteract()
    {
        // Check for points
        if (PlayerStatus.instance.shooting.CheckIfWeaponOwned(weapon) && weapon is MeleeWeapon)
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
            if (!repeatable)
            {
                interactable = false;
                interactIcon.Hide();
            }
        }
    }
}