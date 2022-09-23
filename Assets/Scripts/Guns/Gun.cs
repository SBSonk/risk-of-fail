using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class Gun : Weapon
{
    public float bulletVelocity;
    public int pierceAmount = 0; // Amount of surfaces it can pass through before ending
    public GameObject bullet;

    public static GameObject lastBulletShot;

    public override void ShootWeapon(Transform player)
    {
        // Spawn projectile
        lastBulletShot = SpawnBullet(player);
        Rigidbody2D bulletShot = lastBulletShot.GetComponent<Rigidbody2D>();

        // Apply velocity to bullet
        bulletShot.AddForce(bulletShot.transform.right.normalized * bulletVelocity, ForceMode2D.Impulse);
        bulletShot.AddForce(bulletShot.transform.up * ((Mathf.PerlinNoise(player.position.x * Time.time, player.position.y * Time.time) - .5f) * bulletSpread), ForceMode2D.Impulse);
    }

    // Spawns and returns the bullet gameobject
    protected virtual GameObject SpawnBullet(Transform player)
    {
        // Spawn projectile
        GameObject bulletShot = Instantiate(bullet, position: player.position, rotation: player.rotation);

        // Pass on bullet damage
        Projectile projectile = bulletShot.GetComponent<Projectile>();
        projectile.damage = baseDamage;
        projectile.knockback = knockbackAmount;
        projectile.stunLength = stunLength;
        projectile.shooter = player.gameObject;
        projectile.pierces = pierceAmount;
        projectile.bulletShake = hitScreenShake;

        return bulletShot;
    }
}
