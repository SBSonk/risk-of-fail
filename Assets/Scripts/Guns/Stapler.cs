using System.Collections;
using System.Collections.Generic;
using FirstGearGames.SmoothCameraShaker;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stapler", menuName = "Weapons/Stapler")]
public class Stapler : MeleeWeapon
{
    public float bulletVelocity;
    public int pierceAmount = 1; // Amount of surfaces it can pass through before ending
    public Projectile bullet;
    
    public override IEnumerator SwingWeapon(Transform player, float multiplier = 1)
    {
        // Spawn projectile
        Gun.lastBulletShot = SpawnBullet(player, multiplier);
        Rigidbody2D bulletShot = Gun.lastBulletShot.GetComponent<Rigidbody2D>();

        // Apply velocity to bullet
        bulletShot.AddForce(bulletShot.transform.right.normalized * bulletVelocity, ForceMode2D.Impulse);
        bulletShot.AddForce(bulletShot.transform.up * ((Mathf.PerlinNoise(player.position.x * Time.time, player.position.y * Time.time) - .5f) * bulletSpread), ForceMode2D.Impulse);

        if (shootShake) CameraShakerHandler.Shake(shootShake);
        
        return base.SwingWeapon(player, multiplier);
    }
    
    protected virtual GameObject SpawnBullet(Transform player, float multiplier = 1)
    {
        // Spawn projectile
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 hitVector = (mousePosition - player.position).normalized;
        var projectile = Instantiate(bullet, position: player.position, rotation: player.rotation);
        projectile.transform.right = hitVector;

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
