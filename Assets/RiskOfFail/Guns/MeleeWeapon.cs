using System.Collections;
using System.Collections.Generic;
using FirstGearGames.SmoothCameraShaker;
using GameAudioScriptingEssentials;
using RiskOfFail.Combat;
using RiskOfFail.Combat.Enums;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee", menuName = "Weapons/Melee")]
public class MeleeWeapon : Weapon
{
    public Projectile friendlyQuizBullet;

    public float swingDelay = 0.1f;
    public float hitArea = 2;
    public float hitDistanceMultiplier = 1f;
    public ContactFilter2D hitFilter;

    public AudioClipRandomizer soundPrefab;

    public virtual IEnumerator SwingWeapon(Transform player, float multiplier = 1)
    {
        yield return new WaitForSeconds(swingDelay);

        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var hitVector = (mousePosition - player.position).normalized;

        // Attack check
        var col = Physics2D.OverlapCircleAll(player.position + hitVector * hitDistanceMultiplier + Vector3.up, hitArea);
        List<Alive> hit = new();
        foreach (var c in col)
            if (c.TryGetComponent(out Alive a) && !c.CompareTag("Player") && !hit.Contains(a))
            {
                var objectHit = Physics2D.Linecast(player.position, a.transform.position, hitFilter.layerMask);
                if (objectHit && objectHit.collider.CompareTag("Wall")) continue;

                a.GiveDamage(baseDamage * multiplier, stunLength, DamageTypeFlag.Melee);
                if (a.TryGetComponent(out Rigidbody2D rb))
                    rb.AddForce(hitVector.normalized * knockbackAmount, ForceMode2D.Impulse);

                hit.Add(a);

                LevelStats.main.GiveDamage(baseDamage * multiplier);
            }
            else if (c.TryGetComponent(out Ball b))
            {
                b.GetComponent<Rigidbody2D>().AddForce(hitVector * 15, ForceMode2D.Impulse);
            }

        // PARRY
        var parryHits = Physics2D.OverlapCircleAll(player.position + hitVector, hitArea * 1.25f);
        foreach (var c in parryHits)
            if (c.TryGetComponent(out Rigidbody2D _rb) && c.TryGetComponent(out Projectile p) && p.active)
            {
                // Spawn Bullet
                var bullet = Instantiate(friendlyQuizBullet, player.position + hitVector, quaternion.identity);
                bullet.transform.right = hitVector;

                bullet.GetComponent<Rigidbody2D>().linearVelocity = bullet.transform.right.normalized * 30;

                // Delete Bullet
                Destroy(_rb.gameObject);
            }


        if (shootShake) CameraShakerHandler.Shake(shootShake);
        if (hit.Count > 0)
        {
            // Screenshake
            if (hitScreenShake) CameraShakerHandler.Shake(hitScreenShake);

            // Sound
            if (effects.enemyHitSounds.Length != 0 && soundPrefab)
            {
                var hitSound = Instantiate(soundPrefab);
                hitSound.SetAudioClips(effects.enemyHitSounds);

                if (hitSound.HasAudioClips()) hitSound.PlaySFX();
            }
        }
    }
}