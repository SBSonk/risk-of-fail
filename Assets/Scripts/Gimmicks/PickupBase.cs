using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Events;

public class PickupBase : MonoBehaviour
{
    [SerializeField] float lifetime = 5;

    [SerializeField] protected ParticleSystem particles;
    [SerializeField] protected Light2D _light;
    [SerializeField] protected GameObject sprite;

    public UnityEvent OnPickupCollect;

    protected virtual void Start()
    {
        // Destroy if not collected
        Destroy(transform.parent.gameObject, lifetime);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        // Return if the player isnt the one who collected
        if (!other.CompareTag("Player")) return;

        transform.position = other.transform.position;
        PlayPickupAnimation();
    }

    protected void PlayPickupAnimation()
    {
        OnPickupCollect?.Invoke();

        // Disable hitbox to remove double collision
        GetComponent<Collider2D>().enabled = false;

        // Play particle animation and hide object
        _light.intensity = 0;
        sprite.SetActive(false);
        particles.Play();

        // Destroy pickup
        Destroy(transform.parent.gameObject, 1f);
    }
}
