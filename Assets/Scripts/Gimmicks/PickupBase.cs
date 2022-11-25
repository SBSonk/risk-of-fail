using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Events;
using System.Collections;

public class PickupBase : MonoBehaviour
{
    [SerializeField] float lifetime = 5;

    [SerializeField] protected ParticleSystem particles;
    [SerializeField] protected Light2D _light;
    [SerializeField] protected GameObject sprite;
    public bool active = true;

    public UnityEvent OnPickupCollect;

    Attract attract;

    private void Awake()
    {
        attract = GetComponentInParent<Attract>();
    }

    protected virtual void Start()
    {
        // Destroy if not collected
        Invoke("StartFlicker", lifetime - 3);
        Destroy(transform.parent.gameObject, lifetime);
    }

    void StartFlicker()
    {
        StartCoroutine(SprFunctions.Flicker(sprite.GetComponent<SpriteRenderer>(), 3));
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!active) return;

        if (!other.CompareTag("Player")) return;

        transform.position = other.transform.position;
        PlayPickupAnimation();

        active = false;
    }

    protected void PlayPickupAnimation()
    {
        StopAllCoroutines();
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

    public void ActivateInSeconds(float seconds)
    {
        StartCoroutine(Activate(seconds));
    }

    IEnumerator Activate(float s)
    { 
        active = false;
        attract._collider.enabled = false;
        yield return new WaitForSeconds(s);
        active = true;
    }
}
