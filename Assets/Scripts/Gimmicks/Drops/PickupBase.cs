using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Events;
using System.Collections;
using GameAudioScriptingEssentials;

public class PickupBase : MonoBehaviour
{
    [SerializeField] int pickupPoints = 25;
    [SerializeField] float lifetime = 5;

    [SerializeField] protected ParticleSystem particles;
    [SerializeField] protected Light2D _light;
    [SerializeField] protected GameObject sprite;
    [SerializeField] protected AudioClipRandomizer collectAudio;
    public bool active = true;

    public UnityEvent OnPickupCollect;

    Attract attract;

    private void Awake()
    {
        attract = GetComponentInParent<Attract>();
    }

    protected virtual void Start()
    {
        if (lifetime > 0)
        {
            // Destroy if not collected
            Invoke("StartFlicker", lifetime - 3);
            Destroy(transform.parent.gameObject, lifetime);
        }
        
        ActivateInSeconds(0.5f);
    }

    void StartFlicker()
    {
        StartCoroutine(SprFunctions.Flicker(sprite.GetComponent<SpriteRenderer>(), 3));
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
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
        LevelStats.main.GiveScore(pickupPoints);
        OnPickupCollect?.Invoke();

        // Disable hitbox to remove double collision
        GetComponent<Collider2D>().enabled = false;

        // Play particle animation and hide object
        _light.intensity = 0;
        sprite.SetActive(false);
        particles.Play();

        // Sound
        collectAudio.PlaySFX();

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
        yield return new WaitForSeconds(s);
        active = true;
    }
}
