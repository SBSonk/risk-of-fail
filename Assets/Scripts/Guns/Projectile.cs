using System;
using FirstGearGames.SmoothCameraShaker;
using GameAudioScriptingEssentials;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    [SerializeField] private bool isPlayerBullet = false;
    [SerializeField] private Transform hitmarkerOrigin;
    [FormerlySerializedAs("hitmarker")] public GameObject hitmarkerPrefab;
    [SerializeField] Light2D _light;
    public Rigidbody2D rb;
    public GameObject sprite, shooter;
    public bool keepAligned = false;
    
    public float damage = 100, knockback = 10f, stunLength; // Should be set by the shooter script once instantiated
    public int pierces = 0;
    public ShakeData bulletShake;

    public PaintSplatter paintPrefab;

    public bool active = true;
    List<int> objectsHit = new List<int>();

    public AudioClipRandomizer audioRandomizer;

    public AudioClip[] _enemyHit, _wallHit;

    public void Initialize(AudioClip[] enemyHit, AudioClip[] wallHit)
    {
        _enemyHit = enemyHit;
        _wallHit = wallHit;
    }

    protected virtual void Update()
    {
        if (!keepAligned) return;
        
        sprite.transform.rotation = quaternion.identity;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!active || objectsHit.Contains(collision.gameObject.GetInstanceID())) return;

        if (collision.CompareTag("CanBePenetrated")) return;
        if (collision.isTrigger) return;

        objectsHit.Add(collision.gameObject.GetInstanceID());
        if (collision.CompareTag("Projectile") || ReferenceEquals(collision.gameObject, shooter)) return;

        if (collision.GetComponent<Alive>())
        {
            if (collision.GetComponent<Enemy>() || collision.GetComponent<BossEnemy>()) LevelStats.main.BulletHit(); // Should prolly use an event instead  
            
            // Screenshake
            if (bulletShake) CameraShakerHandler.Shake(bulletShake);

            // Give damage and stun
            collision.GetComponent<Alive>().GiveDamage(damage, stunLength, KillFlag.Ranged);

            collision.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.right * knockback, collision.ClosestPoint(transform.position), ForceMode2D.Impulse);
            
            if (isPlayerBullet) LevelStats.main.GiveDamage(damage);
        }

        // Reduce damage every pierce
        Instantiate(hitmarkerPrefab, hitmarkerOrigin.position, hitmarkerOrigin.rotation);
        damage /= 1.25f;

        if (collision.CompareTag("Alive")) pierces--;

        if (paintPrefab) Instantiate(paintPrefab, transform.position, Quaternion.identity);
        audioRandomizer.SetAudioClips(collision.CompareTag("Wall") ? _wallHit : _enemyHit);
        if (audioRandomizer.HasAudioClips()) audioRandomizer.PlaySFX();
        
        // Piercing
        if (pierces <= 0 || collision.CompareTag("Wall"))
        {
            rb.linearVelocity = Vector2.zero;
            /*if (!collision.CompareTag("Wall"))*/ sprite.SetActive(false);
            _light.intensity = 0;

            // Destroy bullet
            active = false;
            Destroy(gameObject, 1f);
        }
    }
}