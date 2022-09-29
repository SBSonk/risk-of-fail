using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] Transform gunPivot, gunBarrel;

    public inventoryWeapon fallbackWep;
    List<inventoryWeapon> weaponPool;
    int currentWeaponIndex = 0;

    public bool reloading;
    bool canShoot = true;

    [Header("Shoving")]
    [SerializeField] float shoveStrength;
    [SerializeField] float shoveStunTime = 1f;
    [SerializeField] float shoveCooldown = 1f;
    [SerializeField] float radius;
    [SerializeField] float distance;
    [SerializeField] bool canShove = true;

    // EVENTS
    public delegate void OnWeaponSwitch();
    public static event OnWeaponSwitch onWeaponSwitch;

    public delegate void OnPlayerShoot();
    public static event OnPlayerShoot onPlayerShoot;

    public delegate void OnAmmoUpdate();
    public static event OnAmmoUpdate onAmmoUpdate;

    public delegate void OnReloadStart();
    public static event OnReloadStart onReloadStart;

    public delegate void OnShove();
    public static event OnShove onShove;

    private void Start()
    {
        // Grab inventory from playerData
        weaponPool = GameManager.main.pData.weaponsOwned;

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

        if (weaponPool.Count > 1) WeaponSwitching();
        else if (weaponPool.Count == 0) return;

        GunBehavior(GetHeldWeapon().weapon as Gun); // shouldnt be casting every frame

        Shoving();

        // Reloading
        if (GetHeldWeapon().clip < GetHeldWeapon().weapon.clipSize && InputManager.reload && reloading == false) StartCoroutine(Reload());
    }

    void WeaponSwitching()
    {
        if (InputManager.swapWeapon == 0) return;

        // Weapon swapping, Everything unfucked...
        currentWeaponIndex += InputManager.swapWeapon;

        // Dont allow weapon index to go below or above weapon count
        currentWeaponIndex %= weaponPool.Count;
        if (Mathf.Sign(currentWeaponIndex) < 0) currentWeaponIndex = weaponPool.Count - 1;

        // Disable reloading
        canShoot = true;
        reloading = false;

        if (onWeaponSwitch != null) onWeaponSwitch.Invoke();
    }

    void Shoving()
    {
        if (!InputManager.shove || !canShove) return;

        if (onShove != null) onShove.Invoke();

        // Check all objects in radius in front of shootpivot
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius, gunBarrel.right, distance);
        if (hits.Length == 0) return;

        // Check if theyre alive
        foreach (RaycastHit2D h in hits)
        {
            Enemy enemy = h.collider.GetComponent<Enemy>();
            if (enemy)
            {
                print(enemy.gameObject.name);

                // Stun enemy
                enemy.Stun(shoveStunTime);

                // Knockback
                enemy.GetComponent<Rigidbody2D>().AddForce(gunBarrel.right.normalized * shoveStrength, ForceMode2D.Impulse);
            }
        }

        // Cooldown
        canShove = false;
        Invoke("EnableShove", shoveCooldown);
    }

    // Handle shooting of GUN type weapons
    void GunBehavior(Gun g)
    {
        // Don't let player shoot if reloading
        if (!canShoot) return;

        // Check if firing semi auto or full auto
        bool shooting = false;
        if (g.auto) shooting = InputManager.shootAuto;
        else shooting = InputManager.shoot;

        // Cancel if not shooting
        if (!shooting) return;

        // Shooting and magazines
        var weapon = GetHeldWeapon();
        if (g.clipSize > 0)
        {
            // Remove ammo in clip if the gun uses clips
            if (weapon.clip >= g.ammoPerShot)
            {
                ShootGun(g);

                weapon.clip -= g.ammoPerShot;

                if (onAmmoUpdate != null) onAmmoUpdate.Invoke();
            }

            // Autoreload if no ammo
            if (weapon.clip == 0) StartCoroutine(Reload());
        }
        else
        {
            if (weapon.pool >= g.ammoPerShot)
            {
                ShootGun(g);

                if (onPlayerShoot != null) onPlayerShoot.Invoke();

                weapon.pool -= g.ammoPerShot;
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
        if (onReloadStart != null) onReloadStart.Invoke();

        canShoot = false;
        reloading = true;

        // Put back spare ammo in magazine to pool
        weaponPool[currentWeaponIndex].pool += weaponPool[currentWeaponIndex].clip;
        weaponPool[currentWeaponIndex].clip = 0;

        yield return new WaitForSeconds(weaponPool[currentWeaponIndex].weapon.reloadLength);

        // Cancel if stopped reloading
        if (reloading)
        {
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

    void EnableShove()
    {
        canShove = true;
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

    public inventoryWeapon GetHeldWeapon()
    {
        if (weaponPool.Count == 0) return fallbackWep;
        return weaponPool[currentWeaponIndex];
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