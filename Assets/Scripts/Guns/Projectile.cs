using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Projectile : MonoBehaviour
{
    [SerializeField] ParticleSystem hitmarker;
    [SerializeField] Light2D     _light;
    public Rigidbody2D rb;
    public GameObject sprite, shooter;

    public float damage = 100, knockback = 10f, stunLength; // Should be set by the shooter script once instantiated
    public int pierces = 0;
    public ScreenshakeValue bulletShake;

    bool active = true;
    List<int> objectsHit = new List<int>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!active || objectsHit.Contains(collision.gameObject.GetInstanceID())) return;

        // Stop colliding again with the same object
        objectsHit.Add(collision.gameObject.GetInstanceID());

        // Shouldn't collide with self or other bullets
        if (collision.CompareTag("Projectile") || ReferenceEquals(collision.gameObject, shooter)) return;

        // Hit something that isnt the one who shot
        if (collision.GetComponent<Alive>())
        {
            // Screenshake
            CameraFunctions.main.DoScreenShake(bulletShake);

            // Give damage and stun
            collision.GetComponent<Alive>().GiveDamage(damage, stunLength);

            // Give knockback TODO: reduce knockback the longer the bullet is alive
            collision.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.right * knockback, collision.ClosestPoint(transform.position), ForceMode2D.Impulse);
        }

        // Hitmarker effect
        hitmarker.Play();

        // Piercing
        if (pierces == 0 || collision.CompareTag("Environment"))
        {
            rb.velocity = Vector2.zero;
            sprite.SetActive(false);
            _light.intensity = 0;

            // Destroy bullet
            active = false;
            Destroy(gameObject, 1f);
        }

        // Reduce damage every pierce
        damage /= 1.25f;

        pierces--;
    }
}
