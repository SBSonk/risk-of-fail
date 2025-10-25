using FirstGearGames.SmoothCameraShaker;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Weapons/Gun")]
public class Gun : Weapon
{
    public static GameObject lastBulletShot;
    public float bulletVelocity;
    public int pierceAmount; // Amount of surfaces it can pass through before ending
    public Projectile bullet;
    public AmmoDrops ammoDrop;

    public override void ShootWeapon(Transform player, float multiplier = 1)
    {
        // Spawn projectile
        lastBulletShot = SpawnBullet(player, multiplier);
        var bulletShot = lastBulletShot.GetComponent<Rigidbody2D>();

        // Apply velocity to bullet
        bulletShot.AddForce(bulletShot.transform.right.normalized * bulletVelocity, ForceMode2D.Impulse);
        bulletShot.AddForce(
            bulletShot.transform.up *
            ((Mathf.PerlinNoise(player.position.x * Time.time, player.position.y * Time.time) - .5f) * bulletSpread),
            ForceMode2D.Impulse);

        if (shootShake) CameraShakerHandler.Shake(shootShake);
    }

    // Spawns and returns the bullet gameobject
    protected virtual GameObject SpawnBullet(Transform player, float multiplier = 1)
    {
        // Spawn projectile
        var projectile = Instantiate(bullet, player.position, player.rotation);

        // Pass on bullet damage
        projectile.damage = baseDamage * multiplier;
        projectile.knockback = knockbackAmount;
        projectile.stunLength = stunLength;
        projectile.shooter = player.gameObject;
        projectile.pierces = pierceAmount;
        projectile.bulletShake = hitScreenShake;

        // Pass HitAudio
        projectile.Initialize(effects.enemyHitSounds, effects.wallHitSounds);

        return projectile.gameObject;
    }
}