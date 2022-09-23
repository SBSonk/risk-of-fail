using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "QuizWeapon", menuName = "QuizWeapon")]
public class QuizWeapon : Gun
{
    public int minDamage;
    public Color lowColor = Color.red, highColor = Color.green;

    protected override GameObject SpawnBullet(Transform player)
    {
        // Spawn projectile
        GameObject bulletShot = Instantiate(bullet, position: player.position, rotation: player.rotation);

        // Pass on bullet stats
        Projectile projectile = bulletShot.GetComponent<Projectile>();
        projectile.knockback = knockbackAmount;
        projectile.stunLength = stunLength;
        projectile.shooter = player.gameObject;
        projectile.pierces = pierceAmount;
        projectile.bulletShake = hitScreenShake;

        // Decide damage count
        float damage = Mathf.RoundToInt(Random.Range(minDamage, baseDamage));
        projectile.damage = damage;

        // Apply damage count to bullet
        TextMeshPro text = bulletShot.GetComponentInChildren<TextMeshPro>();
        text.text = damage.ToString();

        // TODO: Change bullet color from red to green depending on its final damage percentage compared to baseDamage
        Color color = Color.Lerp(lowColor, highColor, (float) System.Math.Round(damage / baseDamage, 2));
        text.color = color;
        

        return bulletShot;
    }
}
