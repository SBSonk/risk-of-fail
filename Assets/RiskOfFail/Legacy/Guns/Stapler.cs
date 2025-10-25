using System.Collections;
using System.Collections.Generic;
using FirstGearGames.SmoothCameraShaker;
using RiskOfFail.Combat;
using RiskOfFail.Combat.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stapler", menuName = "Weapons/Stapler")]
public class Stapler : MeleeWeapon
{
    public float bulletDamage = 5;
    public Vector3 bulletOffset;
    public int amountOfBullets;
    public float timeBetweenBullets;
    public float bulletVelocity;
    public int pierceAmount = 1; // Amount of surfaces it can pass through before ending
    public Projectile bullet;

    public override IEnumerator SwingWeapon(Transform player, float multiplier = 1)
    {
        yield return new WaitForSeconds(swingDelay);

        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var hitVector = (mousePosition - player.position).normalized;

        // Attack check
        var col = Physics2D.OverlapCircleAll(player.position + hitVector * hitArea, hitArea);
        List<Alive> hit = new();
        foreach (var c in col)
            if (c.TryGetComponent(out Alive a) && !c.CompareTag("Player") && !hit.Contains(a))
            {
                var objectHit = Physics2D.Linecast(player.position, a.transform.position, hitFilter.layerMask);
                if (objectHit && objectHit.collider.CompareTag("Wall")) continue;

                a.GiveDamage(baseDamage * multiplier, stunLength, DamageTypeFlag.Melee);
                if (a.TryGetComponent(out Rigidbody2D rb))
                    rb.AddForce(hitVector * knockbackAmount, ForceMode2D.Impulse);

                hit.Add(a);

                LevelStats.main.GiveDamage(baseDamage * multiplier);
            }
            else if (c.TryGetComponent(out Ball b))
            {
                b.GetComponent<Rigidbody2D>().AddForce(hitVector * 15, ForceMode2D.Impulse);
            }

        for (var i = 0; i < amountOfBullets; i++)
        {
            // Spawn projectile
            Gun.lastBulletShot = SpawnBullet(player, multiplier);
            var bulletShot = Gun.lastBulletShot.GetComponent<Rigidbody2D>();

            // Apply velocity to bullet
            bulletShot.AddForce(bulletShot.transform.right.normalized * bulletVelocity, ForceMode2D.Impulse);
            bulletShot.AddForce(
                bulletShot.transform.up *
                ((Mathf.PerlinNoise(player.position.x * Time.time, player.position.y * Time.time) - .5f) *
                 bulletSpread), ForceMode2D.Impulse);

            if (shootShake) CameraShakerHandler.Shake(shootShake);

            yield return new WaitForSeconds(timeBetweenBullets);
        }
    }

    protected virtual GameObject SpawnBullet(Transform player, float multiplier = 1)
    {
        // Spawn projectile
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var hitVector = (mousePosition - player.position).normalized;
        var projectile = Instantiate(bullet, player.position + hitVector + bulletOffset, player.rotation);

        var x = mousePosition.x - projectile.transform.position.x;
        var y = mousePosition.y - projectile.transform.position.y;

        var angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Pass on bullet damage
        projectile.damage = bulletDamage * multiplier;
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