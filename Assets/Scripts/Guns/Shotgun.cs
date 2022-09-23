using UnityEngine;

[CreateAssetMenu(fileName = "New Shotgun", menuName = "Shotgun")]
public class Shotgun : Gun
{
    public int pelletCount = 3;
    public float pelletSpread = 25; // Pellet angle in degrees

    public override void ShootWeapon(Transform player)
    {
        for (int i = -pelletCount; i < pelletCount; i++)
        {
            // Spawn projectile
            GameObject bulletShot = SpawnBullet(player);
            Rigidbody2D rb = bulletShot.GetComponent<Rigidbody2D>();

            // Iterate spawn position
            Vector3 spawnPosition = player.position;
            Vector3 spawnRotation = player.rotation.eulerAngles + new Vector3(0, 0, pelletSpread * i);

            // Offset bullet
            bulletShot.transform.position = spawnPosition;
            bulletShot.transform.rotation = Quaternion.Euler(spawnRotation);

            // Apply velocity to bullet
            rb.AddForce(bulletShot.transform.right.normalized * bulletVelocity, ForceMode2D.Impulse);
            rb.AddForce(bulletShot.transform.up * ((Mathf.PerlinNoise(player.position.x * Time.time, player.position.y * Time.time) - .5f) * bulletSpread), ForceMode2D.Impulse);
        }

        base.ShootWeapon(player);
    }
}
