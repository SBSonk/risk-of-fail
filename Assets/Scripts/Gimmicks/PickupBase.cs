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
    protected bool active = true;

    public UnityEvent OnPickupCollect;

    protected virtual void Start()
    {
        // Destroy if not collected
        Invoke("StartFlicker", lifetime - 3);
        Destroy(transform.parent.gameObject, lifetime);
    }

    void StartFlicker()
    {
        StartCoroutine(FlickerAnimation(3));
    }

    IEnumerator FlickerAnimation(float t)
    {
        float flickerInt = .15f;
        float fastFlickerInt = flickerInt / 1.5f;
        float reallyFastFlickerInt = flickerInt / 2;
        float timeRemaining = t;
        float time20 = t * .4f;
        float time10 = t * .2f;
        // Flicker animation
        bool active = true;
        while (timeRemaining > 0)
        {
            active = !active;
            sprite.SetActive(active);   

            yield return new WaitForSeconds(flickerInt);
            timeRemaining -= flickerInt;

            if (timeRemaining <= time10) flickerInt = reallyFastFlickerInt;
            else if (timeRemaining <= time20) flickerInt = fastFlickerInt;

        }

        sprite.SetActive(true);
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
}
