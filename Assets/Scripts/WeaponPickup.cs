using UnityEngine;
public class WeaponPickup : PickupBase
{
    public Weapon weaponToGive;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        // Return if the player isnt the one who collected
        if (!other.CompareTag("Player")) return;

        // Don't give if already owned
        if (!GameManager.CheckIfWeaponOwned(weaponToGive)) GameManager.GiveWeapon(weaponToGive);

        PlayPickupAnimation();
    }
}
