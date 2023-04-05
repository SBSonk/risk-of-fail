using System;
using System.Collections.Generic;
using GameAudioScriptingEssentials;
using UnityEngine;
using UnityEngine.Events;

public abstract class WalkableTrap : MonoBehaviour
{
    [SerializeField] protected AudioClipRandomizer triggerSound, damageSound;

    protected List<Alive> entitiesInsideArea;
    public UnityEvent OnTrapTriggered, OnTrapLeft, OnTrapStay;

    bool active = true;

    private void Start()
    {
        entitiesInsideArea = new List<Alive>();
    }
    
    protected virtual void DoTrapDamage() {}
    protected virtual void TrapStayUpdate() {}
    protected virtual void TrapExit() {}
    

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Alive>(out var alive))
        {
            entitiesInsideArea.Add(alive);
        }

        if (!collision.CompareTag("Player") || !active) return;

        active = false; 
        
        OnTrapTriggered?.Invoke();
        if (triggerSound) triggerSound.PlaySFX();
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        TrapStayUpdate();
        OnTrapStay?.Invoke();
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Alive>(out var alive) && entitiesInsideArea.Contains(alive))
        {
            entitiesInsideArea.Remove(alive);
        }

        TrapExit();
        OnTrapLeft?.Invoke();
    }

    // Wait for player to enter

    // Start timer

    // Wait for timer

    // Check if player is still in trigger zone when timer stops

    // Damage player
}