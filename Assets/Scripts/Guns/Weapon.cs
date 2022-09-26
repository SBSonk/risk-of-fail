using UnityEngine;

public class Weapon : ScriptableObject
{
    [Header("Metadata")]
    public string weaponName = "Untitled Weapon";
    //added for Shop UI
    public string weaponDescription;
    public int weaponCost;
    public int weaponStrength;
    public int weaponFireRate;
    public int weaponPiercing;
    public bool isWeaponObtained = false;
    //end
    public Sprite hudElement;
    public Sprite crossHair;
    public Sprite weaponSprite;

    [Header("Weapon Stats")]
    public float baseDamage;
    public float bulletSpread = 0f;
    public float knockbackAmount = 10f;
    public float stunLength = 0.25f; // In seconds
    public int defaultAmmoCount = 100;
    public ScreenshakeValue hitScreenShake;

    [Header("Weapon Behavior")]
    public int ammoPerShot = 0;
    public int clipSize = -1; // -1 to disable reloading
    public float reloadLength = 0; // Reload time in seconds   
    public float fireRate = 1f; // Firerate in seconds
    public bool auto = false; // Determines if you can hold left click
    
    // Left click attack
    public virtual void ShootWeapon(Transform player)
    {
        return;
    }

    // Right click attack
    public virtual void AltShoot(Transform player)
    {
        return;
    }
}
