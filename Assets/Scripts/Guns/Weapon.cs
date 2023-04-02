using FirstGearGames.SmoothCameraShaker;
using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    public ShopMetadata shopData;
    public HUDElement hud;

    public Sprite weaponSprite;

    [Header("Weapon Stats")]
    public float baseDamage;
    public float bulletSpread = 0f;
    public float knockbackAmount = 10f;
    public float stunLength = 0.25f; // In seconds
    public int defaultAmmoCount = 100;

    [Header("Weapon Behavior")]
    public int ammoPerShot = 0;
    public int clipSize = -1; // -1 to disable reloading
    public float reloadLength = 0; // Reload time in seconds   
    public float fireRate = 1f; // Firerate in seconds
    public bool auto = false; // Determines if you can hold left click
    public ShakeData shootShake, hitScreenShake;

    [Header("Weapon Animations")]
    public WeaponSFX effects;
    public Vector3 weaponScale = Vector3.one;
    public Vector3 weaponOffset;
    public WeaponAnimator animatorController;
    public Rigidbody2D rigidbodyVariant;

    // Left click attack
    public virtual void ShootWeapon(Transform player, float multiplier = 1)
    {
        return;
    }
}   

[System.Serializable]
public struct HUDElement
{
    public Sprite sprite;
    public Vector2 offset;

    public Crosshair crossHair;
}

[System.Serializable]
public struct ShopMetadata
{
    public string weaponDescription;
    public int weaponCost;
    [Range(0, 4)] public int weaponStrength;
    [Range(0, 4)] public int weaponFireRate;
    [Range(0, 4)] public int weaponPiercing;
}

[System.Serializable]
public struct WeaponSFX
{
    public AudioClip[] shootSounds, reloadSounds, shoveSounds;
    public AudioClip[] enemyHitSounds, wallHitSounds;
}

// think im using too many structs