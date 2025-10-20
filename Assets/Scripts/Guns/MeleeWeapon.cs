using FirstGearGames.SmoothCameraShaker;
using System.Collections;
using System.Collections.Generic;
using GameAudioScriptingEssentials;
using Unity.Mathematics;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Rendering;

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

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 hitVector = (mousePosition - player.position).normalized;

        // Attack check
        Collider2D[] col = Physics2D.OverlapCircleAll(player.position + (hitVector * hitDistanceMultiplier) + Vector3.up, hitArea);
        List<Alive> hit = new List<Alive>();
        foreach(Collider2D c in col)
        {
            if (c.TryGetComponent(out Alive a) && !c.CompareTag("Player") && !hit.Contains(a))
            {
                var objectHit = Physics2D.Linecast(player.position, a.transform.position, hitFilter.layerMask);
                if (objectHit && objectHit.collider.CompareTag("Wall")) continue;
                
                a.GiveDamage(baseDamage * multiplier, stunLength, KillFlag.Melee);
                if (a.TryGetComponent(out Rigidbody2D rb))
                {
                    rb.AddForce(hitVector.normalized * knockbackAmount, ForceMode2D.Impulse);
                }

                hit.Add(a);
                
                LevelStats.main.GiveDamage(baseDamage * multiplier);
            } else if (c.TryGetComponent(out Ball b))
            {
                b.GetComponent<Rigidbody2D>().AddForce(hitVector * 15, ForceMode2D.Impulse);
            }
        }

        // PARRY
        Collider2D[] parryHits = Physics2D.OverlapCircleAll(player.position + hitVector, hitArea * 1.25f);
        foreach(Collider2D c in parryHits)
        {
            if (c.TryGetComponent(out Rigidbody2D _rb) && c.TryGetComponent(out Projectile p) && p.active)
            {
                // Spawn Bullet
                Projectile bullet = Instantiate(friendlyQuizBullet, player.position + hitVector, quaternion.identity);
                bullet.transform.right = hitVector;

                bullet.GetComponent<Rigidbody2D>().linearVelocity = bullet.transform.right.normalized * 30;

                // Delete Bullet
                Destroy(_rb.gameObject);
            }
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
