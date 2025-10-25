using System.Collections;
using RiskOfFail.Combat;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shotgun", menuName = "Weapons/Shotgun")]
public class Shotgun : Gun
{
    public int pelletCount = 3;
    public float pelletSpread = 25; // Pellet angle in degrees
    public float perBulletDelay;
    public float shootDelay;

    public override void ShootWeapon(Transform player, float multiplier = 1)
    {
        PlayerStatus.player.pShooting.StartCoroutine(ShootPerBullet(player, multiplier));
    }

    private IEnumerator ShootPerBullet(Transform player, float multiplier = 1)
    {
        yield return new WaitForSeconds(shootDelay);

        for (var i = pelletCount; i > -pelletCount; i--)
        {
            // Spawn projectile
            var bulletShot = SpawnBullet(player, multiplier);
            var rb = bulletShot.GetComponent<Rigidbody2D>();

            // Iterate spawn position
            var spawnPosition = player.position;
            var spawnRotation = player.rotation.eulerAngles + new Vector3(0, 0, pelletSpread * i);

            // Offset bullet
            bulletShot.transform.position = spawnPosition;
            bulletShot.transform.rotation = Quaternion.Euler(spawnRotation);

            // Apply velocity to bullet
            rb.AddForce(bulletShot.transform.right.normalized * bulletVelocity, ForceMode2D.Impulse);
            rb.AddForce(
                bulletShot.transform.up *
                ((Mathf.PerlinNoise(player.position.x * Time.time, player.position.y * Time.time) - .5f) *
                 bulletSpread), ForceMode2D.Impulse);

            lastBulletShot = bulletShot;
            yield return new WaitForSeconds(perBulletDelay);
        }
    }
}