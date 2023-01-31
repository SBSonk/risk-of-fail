using System.Collections.Generic;
using GameAudioScriptingEssentials;
using UnityEngine;
using UnityEngine.Events;

public abstract class WalkableTrap : MonoBehaviour
{
    [SerializeField] protected DamageSource dmg;
    [SerializeField] protected float timeTillDamage;
    [SerializeField] protected float speedMultiplier = 1;
    [SerializeField] protected AudioClipRandomizer triggerSound, damageSound;

    protected List<Alive> entitiesInsideArea;
    public UnityEvent OnTrapTriggered;

    bool active = true;

    private void Start()
    {
        entitiesInsideArea = new List<Alive>();
    }

    protected abstract void DoTrapDamage();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Alive>(out var alive))
        {
            entitiesInsideArea.Add(alive);
        }

        if (!collision.CompareTag("Player") || !active) return;

        active = false; 
        
        OnTrapTriggered?.Invoke();
        Invoke("DoTrapDamage", timeTillDamage / speedMultiplier);
        if (triggerSound) triggerSound.PlaySFX();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Alive>(out var alive) && entitiesInsideArea.Contains(alive))
        {
            entitiesInsideArea.Remove(alive);
        }
    }

    // Wait for player to enter

    // Start timer

    // Wait for timer

    // Check if player is still in trigger zone when timer stops

    // Damage player
}