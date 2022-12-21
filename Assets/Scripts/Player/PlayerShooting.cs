using GameAudioScriptingEssentials;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] Transform gunPivot, gunBarrel, cursor;

    public InventoryWeapon fallbackWep;
    List<InventoryWeapon> weaponPool;
    int currentWeaponIndex = 0;

    public bool reloading;
    bool canShoot = true;

    [Header("Shoving")]
    [SerializeField] float shoveStrength;
    [SerializeField] float shoveStunTime = 1f;
    [SerializeField] float shoveCooldown = 1f;
    [SerializeField] float radius;

    public UnityEvent<InventoryWeapon> OnWeaponSwitch, OnAmmoUpdate;
    public UnityEvent OnShoot;
    public UnityEvent<float> OnReloadStart;
    public UnityEvent OnMelee, OnShove, OnParry;

    public AudioClipRandomizer reloadingRandomizer;

    public void Initialize()
    {
        // Grab inventory from playerData
        weaponPool = GameManager.main.pData.weaponsOwned;

        // Initialize inventory
        for (int i = 0; i < weaponPool.Count; i++)
        {
            weaponPool[i].Initialize();
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

        if (CheckIfPlayerShooting() && canShoot)
        {
            var weapon = GetHeldWeapon().weapon;
            switch (weapon)
            {
                case Gun g:
                    GunBehavior(g);
                    break;

                case MeleeWeapon m:
                    // Add reload to melee
                    MeleeBehavior(m);
                    break;
            }

            // Apply firerate
            canShoot = false;
            Invoke("EnableShooting", weapon.fireRate);
        }

        Shoving();

        // Reloading
        if (GetHeldWeapon().clip < GetHeldWeapon().weapon.clipSize && InputManager.reload && reloading == false) StartCoroutine(Reload());
    }

    bool CheckIfPlayerShooting()
    {
        if (GetHeldWeapon().weapon.auto) return InputManager.shootAuto;
        else return InputManager.shoot;
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

        OnWeaponSwitch?.Invoke(GetHeldWeapon());
    }

    void Shoving()
    {
        if (!InputManager.shove || !canShoot) return;

        OnShove?.Invoke();

        // Check all objects in radius in front of shootpivot
        Vector3 shoveDir = ((Vector3)InputManager.mousePosition - transform.position).normalized;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + shoveDir * 0.5f, radius);
        
        if (hits.Length == 0) return;

        foreach (Collider2D h in hits)
        {
            if (h.TryGetComponent(out Enemy enemy))
            {
                enemy.Stun(shoveStunTime);
                if (enemy.TryGetComponent(out Rigidbody2D rb))
                {
                    rb.AddForce(shoveDir * shoveStrength, ForceMode2D.Impulse);
                }

            } else if (GetHeldWeapon().weapon is MeleeWeapon && h.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
            {
                foreach (Collider2D c in hits)
                {
                    if (c.TryGetComponent(out Rigidbody2D _rb) && c.TryGetComponent(out Projectile p) && p.active)
                    {
                        _rb.velocity = Vector2.zero;
                        _rb.AddForce(shoveDir * 3, ForceMode2D.Impulse);

                        c.GetComponentInChildren<TrailRenderer>().startColor = Color.cyan;

                        // Convert to playerBullet
                        c.gameObject.layer = LayerMask.NameToLayer("Bullet");
                        p.GetComponent<Projectile>().damage = 60;
                    }

                }

                OnParry?.Invoke();
            }
        }

        canShoot = false;

        CancelInvoke();
        Invoke("EnableShooting", shoveCooldown + GetHeldWeapon().weapon.reloadLength);
    }

    // Handle shooting of GUN type weapons
    void GunBehavior(Gun g)
    {
        // Shooting and magazines
        var weapon = GetHeldWeapon();
        if (g.clipSize > 0)
        {
            // Remove ammo in clip if the gun uses clips
            if (weapon.clip >= g.ammoPerShot)
            {
                ShootGun(g);

                weapon.clip -= g.ammoPerShot;
                OnAmmoUpdate?.Invoke(weapon);
            }

            // Autoreload if no ammo
            if (weapon.clip == 0 && reloading == false) StartCoroutine(Reload());
        }
        else
        {
            if (weapon.pool >= g.ammoPerShot)
            {
                ShootGun(g);

                weapon.pool -= g.ammoPerShot;
                OnAmmoUpdate?.Invoke(weapon);
            }
        }
    }

    void MeleeBehavior(MeleeWeapon m)
    {
        // Shooting and magazines
        var weapon = GetHeldWeapon();
        if (m.clipSize > 0)
        {
            // Remove ammo in clip if the gun uses clips
            if (weapon.clip >= m.ammoPerShot)
            {
                StartCoroutine(m.SwingWeapon(transform));

                weapon.clip -= m.ammoPerShot;
                OnMelee?.Invoke();
            }

            // Autoreload if no ammo
            if (weapon.clip == 0 && reloading == false) StartCoroutine(Reload());
        }
    }

    void ShootGun(Gun g)
    {
        g.ShootWeapon(gunBarrel);

        OnShoot?.Invoke();
    }

    IEnumerator Reload()
    {
        var heldWep = GetHeldWeapon();

        if (heldWep.weapon is MeleeWeapon)
        {
            // Begin Reload
            OnReloadStart?.Invoke(heldWep.weapon.reloadLength);

            canShoot = false;
            reloading = true;

            yield return new WaitForSeconds(heldWep.weapon.reloadLength);

            heldWep.clip = heldWep.weapon.clipSize; 

            reloading = false;
            canShoot = true;
        }
        else
        {
            // Cancel reload if no ammo in pool left
            if (heldWep.pool == 0) yield break;

            // Begin Reload
            OnReloadStart?.Invoke(heldWep.weapon.reloadLength);

            canShoot = false;
            reloading = true;

            // Put back spare ammo in magazine to pool
            weaponPool[currentWeaponIndex].pool += weaponPool[currentWeaponIndex].clip;
            weaponPool[currentWeaponIndex].clip = 0;

            yield return new WaitForSeconds(heldWep.weapon.reloadLength);

            // Cancel if stopped reloading
            if (reloading)
            {
                int amountToReload = heldWep.weapon.clipSize;
                if (amountToReload > heldWep.pool)
                {
                    heldWep.clip = heldWep.pool;
                    heldWep.pool = 0;
                }
                else
                {
                    heldWep.clip = amountToReload;
                    heldWep.pool -= amountToReload;
                }

                OnAmmoUpdate?.Invoke(heldWep);

                yield return new WaitForSeconds(0.5f);

                reloading = false;
                canShoot = true;
            }
        }
    }

    void EnableShooting()
    {
        canShoot = true;
    }

    // Returns the first inventory weapon with type type
    public InventoryWeapon GetWeaponFromInventory(Weapon type)
    {
        foreach (InventoryWeapon w in weaponPool)
        {
            if (w.weapon.name == type.name) return w;
        }

        return null;
    }

    public void GiveAmmo(InventoryWeapon weapon, int amount)
    {
        weapon.pool += amount;

        OnAmmoUpdate?.Invoke(weapon);
    }

    public InventoryWeapon GetHeldWeapon()
    {
        if (weaponPool == null || weaponPool.Count == 0) return fallbackWep;

        return weaponPool[currentWeaponIndex];
    }
}