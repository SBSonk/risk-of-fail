using UnityEngine;

public class Weapon : ScriptableObject
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
    public ScreenshakeValue hitScreenShake;

    [Header("Weapon Behavior")]
    public int ammoPerShot = 0;
    public int clipSize = -1; // -1 to disable reloading
    public float reloadLength = 0; // Reload time in seconds   
    public float fireRate = 1f; // Firerate in seconds
    public bool auto = false; // Determines if you can hold left click

    [Header("Weapon Animations")]
    public AnimationTypes animType = AnimationTypes.Light;
    
    // Left click attack
    public virtual void ShootWeapon(Transform player)
    {
        return;
    }
}   

[System.Serializable]
public enum AnimationTypes
{
    Light = 'A',
    Brush = 'B',
    Heavy = 'C',
    Melee
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