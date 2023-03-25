using GameAudioScriptingEssentials;
using System.Collections;
using System.Collections.Generic;
using FirstGearGames.SmoothCameraShaker;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] Transform gunPivot, gunBarrel;
    public InventoryWeapon fallbackWep;
    List<InventoryWeapon> weaponPool;
    int currentWeaponIndex = 0;

    public float reloadMultiplier = 1;
    public bool reloading;
    private float shootEnableTime;
    public bool canShoot = true;
    public float damageMultiplier = 1;

    [Header("Shoving")]
    [SerializeField] float shoveStrength;
    [SerializeField] float shoveStunTime = 1f;
    [SerializeField] float shoveCooldown = 1f, shoveHitDelay = 0.25f;
    [SerializeField] float radius;
    [SerializeField] private Projectile friendlyQuizBullet;

    public UnityEvent OnWeaponSwitch, OnAmmoUpdate;
    public UnityEvent OnShoot;
    public UnityEvent<float> OnReloadStart;
    public UnityEvent OnMelee, OnShove, OnParry;

    public PlayerSFXManager sfxManager;

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
        if (!IsOwner) return;
        
        // Get direction from cursor to player and make the player face it
        Vector2 mousePos = ((Vector2) transform.position - InputManager.mousePosition).normalized;
        float angle = Mathf.Atan2(-mousePos.y, -mousePos.x) * Mathf.Rad2Deg;
        gunPivot.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (weaponPool.Count > 1) WeaponSwitching();
        else if (weaponPool.Count == 0) return;

        if (CheckIfPlayerShooting() && CanShoot())
        {
            ShootActionServerRpc();
        }

        if (InputManager.shove && CanShoot()) StartCoroutine(Shove());

        // Reloading
        if (GetHeldWeapon().clip < GetHeldWeapon().weapon.clipSize && InputManager.reload && reloading == false) StartCoroutine(Reload());
    }

    [ServerRpc]
    void ShootActionServerRpc()
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
        shootEnableTime = Time.time + weapon.fireRate;
    }
    
    bool CanShoot() => Time.time >= shootEnableTime && canShoot;
    
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

        OnWeaponSwitch?.Invoke();
    }

    IEnumerator Shove()
    {
        sfxManager.PlayShoveSound();
        OnShove?.Invoke();
        
        yield return new WaitForSeconds(shoveHitDelay);
        
        Shoving();
    }
    void Shoving()
    {
        // Check all objects in radius in front of shootpivot
        Vector3 shoveDir = ((Vector3)InputManager.mousePosition - transform.position).normalized;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + shoveDir * radius, radius);
        
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

            } else if (h.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
            {
                foreach (Collider2D c in hits)
                {
                    if (c.TryGetComponent(out Rigidbody2D _rb) && c.TryGetComponent(out Projectile p) && p.active)
                    {
                        // Spawn Bullet
                        Projectile bullet = Instantiate(friendlyQuizBullet, gunBarrel.position, gunBarrel.rotation);

                        bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.right.normalized * 3, ForceMode2D.Impulse);

                        // Delete Bullet
                        Destroy(_rb.gameObject);
                    }

                }

                OnParry?.Invoke();
            }
        }

        float cooldownTime = shoveCooldown + GetHeldWeapon().weapon.reloadLength / 4;
        shootEnableTime = Time.time + cooldownTime;
        
        
        OnReloadStart?.Invoke(cooldownTime);
        /*StopCoroutine(nameof(EnableShooting));
        StartCoroutine(EnableShooting(cooldownTime));*/
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
                sfxManager.PlayShootSound();
                OnAmmoUpdate?.Invoke();
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
                sfxManager.PlayShootSound();
                OnAmmoUpdate?.Invoke();
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
                sfxManager.PlayShootSound();
                OnMelee?.Invoke();
            }

            // Autoreload if no ammo
            if (weapon.clip == 0 && reloading == false) StartCoroutine(Reload());
        }
    }

    void ShootGun(Gun g)
    {
        g.ShootWeapon(gunBarrel, damageMultiplier);

        OnShoot?.Invoke();
    }

    IEnumerator Reload()
    {
        var heldWep = GetHeldWeapon();

        float reloadTime = heldWep.weapon.reloadLength / reloadMultiplier;

        if (heldWep.weapon is MeleeWeapon)
        {
            // Begin Reload
            OnReloadStart?.Invoke(reloadTime);

            canShoot = false;
            reloading = true;

            yield return new WaitForSeconds(reloadTime);

            heldWep.clip = heldWep.weapon.clipSize; 

            reloading = false;
            canShoot = true;
        }
        else
        {
            // Cancel reload if no ammo in pool left
            if (heldWep.pool == 0) yield break;

            // Begin Reload
            sfxManager.PlayReloadSound();
            OnReloadStart?.Invoke(reloadTime);

            canShoot = false;
            reloading = true;

            // Put back spare ammo in magazine to pool
            weaponPool[currentWeaponIndex].pool += weaponPool[currentWeaponIndex].clip;
            weaponPool[currentWeaponIndex].clip = 0;

            yield return new WaitForSeconds(reloadTime);

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

                OnAmmoUpdate?.Invoke();

                yield return new WaitForSeconds(0.5f);

                reloading = false;
                canShoot = true;
            }
        }
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

        OnAmmoUpdate?.Invoke();
    }

    public InventoryWeapon GetHeldWeapon()
    {
        if (weaponPool == null || weaponPool.Count == 0) return fallbackWep;

        return weaponPool[currentWeaponIndex];
    }
}