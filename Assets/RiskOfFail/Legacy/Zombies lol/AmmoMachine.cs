using RiskOfFail.Combat;
using UnityEngine;

public class AmmoMachine : Buyable
{
    protected override void PlayerInteract()
    {
        // Check for points
        if (LevelStats.main.points >= price && PlayerStatus.instance.shooting.GetHeldWeapon().weapon is Gun)
        {
            PlayerStatus.instance.shooting.GiveAmmo(PlayerStatus.instance.shooting.GetHeldWeapon(),
                Mathf.RoundToInt(PlayerStatus.instance.shooting.GetHeldWeapon().weapon.defaultAmmoCount / 1.5f));
            LevelStats.main.GiveScore(-price);
            OnPurchase?.Invoke();

            if (!repeatable)
            {
                interactable = false;
                interactIcon.Hide();
            }
        }
    }
}