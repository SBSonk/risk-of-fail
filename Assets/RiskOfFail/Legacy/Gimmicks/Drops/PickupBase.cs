using System.Collections;
using GameAudioScriptingEssentials;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class PickupBase : MonoBehaviour
{
    [SerializeField] private int pickupPoints = 25;
    [SerializeField] private float lifetime = 5;

    [SerializeField] protected ParticleSystem particles;
    [SerializeField] protected Light2D _light;
    [SerializeField] protected GameObject sprite;
    [SerializeField] protected AudioClipRandomizer collectAudio;
    public bool active = true;

    public UnityEvent OnPickupCollect;

    private Attract attract;

    protected virtual void Awake()
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

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (!active) return;

        if (!other.CompareTag("Player")) return;

        transform.position = other.transform.position;
        PlayPickupAnimation();

        active = false;
    }

    private void StartFlicker()
    {
        StartCoroutine(HelperFunctions.Flicker(sprite.GetComponent<SpriteRenderer>(), 3));
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

    private IEnumerator Activate(float s)
    {
        active = false;
        yield return new WaitForSeconds(s);
        active = true;
    }
}