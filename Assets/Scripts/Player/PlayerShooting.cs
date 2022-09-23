using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] Transform gunPivot, gunBarrel;

    public List<inventoryWeapon> weaponPool;
    public int currentWeaponIndex = 0;

    public bool reloading;
    bool canShoot = true;

    MoveCursor cursor;

    // EVENTS
    public delegate void OnWeaponSwitch();
    public static event OnWeaponSwitch onWeaponSwitch;

    public delegate void OnPlayerShoot();
    public static event OnPlayerShoot onPlayerShoot;

    public delegate void OnAmmoUpdate();
    public static event OnAmmoUpdate onAmmoUpdate;

    private void Awake()
    {
        cursor = GameObject.Find("PlayerCursor").GetComponent<MoveCursor>();
    }

    private void Start()
    {
        // Initialize inventory
        foreach (inventoryWeapon w in weaponPool)
        {
            w.Initialize();
        }
    }

    private void Update()
    {
        // Get direction from cursor to player and make the player face it
        Vector2 mousePos = ((Vector2) transform.position - InputManager.mousePosition).normalized;
        float angle = Mathf.Atan2(-mousePos.y, -mousePos.x) * Mathf.Rad2Deg;
        gunPivot.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Weapon swapping, Everything unfucked...
        if (InputManager.swapWeapon != 0)
        {
            currentWeaponIndex += InputManager.swapWeapon;

            // Dont allow weapon index to go below or above weapon count
            currentWeaponIndex %= weaponPool.Count;
            if (Mathf.Sign(currentWeaponIndex) < 0) currentWeaponIndex = weaponPool.Count - 1;

            // Change Crosshair
            cursor.ChangeCrosshair(weaponPool[currentWeaponIndex].weapon.crossHair);

            // Disable reloading
            canShoot = true;
            reloading = false;

            if (onWeaponSwitch != null) onWeaponSwitch.Invoke();
        }

        // Execute weapon specific attacks
        bool shooting = false;
        if (weaponPool[currentWeaponIndex].weapon.auto) shooting = InputManager.shootAuto;
        else shooting = InputManager.shoot;

        if (shooting)
        {
            switch (weaponPool[currentWeaponIndex].weapon)
            {
                case Gun g:
                    GunBehavior(g);
                    break;
            }
        }

        // Reloading
        if (weaponPool[currentWeaponIndex].clip < weaponPool[currentWeaponIndex].weapon.clipSize && InputManager.reload) StartCoroutine(Reload());
    }

    // Handle shooting of GUN type weapons
    void GunBehavior(Gun g)
    {
        // Don't let player shoot if reloading
        if (!canShoot) return;

        // Shooting and magazines
        if (g.clipSize > 0)
        {
            // Remove ammo in clip if the gun uses clips
            if (weaponPool[currentWeaponIndex].clip >= g.ammoPerShot)
            {
                ShootGun(g);

                weaponPool[currentWeaponIndex].clip -= g.ammoPerShot;

                if (onAmmoUpdate != null) onAmmoUpdate.Invoke();
            }

            // Autoreload if no ammo
            if (weaponPool[currentWeaponIndex].clip == 0) StartCoroutine(Reload());
        }
        else
        {
            if (weaponPool[currentWeaponIndex].pool >= g.ammoPerShot)
            {
                ShootGun(g);

                if (onPlayerShoot != null) onPlayerShoot.Invoke();

                weaponPool[currentWeaponIndex].pool -= g.ammoPerShot;
            }
        }
    }

    void ShootGun(Gun g)
    {
        g.ShootWeapon(gunBarrel);

        // Apply firerate cooldown
        canShoot = false;
        Invoke("EnableShooting", g.fireRate);

        if (onPlayerShoot != null) onPlayerShoot.Invoke();
    }

    IEnumerator Reload()
    {
        // Cancel reload if no ammo in pool left
        if (weaponPool[currentWeaponIndex].pool == 0) yield break;

        // Begin Reload
        canShoot = false;
        reloading = true;
        yield return new WaitForSeconds(weaponPool[currentWeaponIndex].weapon.reloadLength);

        // Cancel if stopped reloading
        if (reloading)
        {
            // Put back spare ammo in magazine to pool
            weaponPool[currentWeaponIndex].pool += weaponPool[currentWeaponIndex].clip;
            weaponPool[currentWeaponIndex].clip = 0;

            int amountToReload = weaponPool[currentWeaponIndex].weapon.clipSize;
            if (amountToReload > weaponPool[currentWeaponIndex].pool)
            {
                weaponPool[currentWeaponIndex].clip = weaponPool[currentWeaponIndex].pool;
                weaponPool[currentWeaponIndex].pool = 0;
            }
            else
            {
                weaponPool[currentWeaponIndex].clip = amountToReload;
                weaponPool[currentWeaponIndex].pool -= amountToReload;
            }

            reloading = false;
            canShoot = true;

            if (onAmmoUpdate != null) onAmmoUpdate.Invoke();
        }
    }

    void EnableShooting()
    {
        canShoot = true;
    }

    // Returns the first inventory weapon with type type
    public inventoryWeapon GetWeaponFromInventory(Weapon type)
    {
        foreach (inventoryWeapon w in weaponPool)
        {
            if (w.weapon.weaponName == type.weaponName) return w;
        }

        return null;
    }

    public void GiveAmmo(inventoryWeapon weapon, int amount)
    {
        weapon.pool += amount;

        if (onAmmoUpdate != null) onAmmoUpdate.Invoke();
    }
}

[System.Serializable] // Used to store inventory weapon values and ammo
public class inventoryWeapon
{
    public Weapon weapon;
    public int clip, pool;

    public inventoryWeapon(Weapon nweapon, int nclip, int npool)
    {
        weapon = nweapon;
        clip = nclip;
        pool = npool;
    }

    public void Initialize()
    {
        clip = weapon.clipSize;
        pool = weapon.defaultAmmoCount;
    }
}