using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class WalkableTrap : MonoBehaviour
{
    [SerializeField] protected DamageSource dmg;
    [SerializeField] protected float timeTillDamage;

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
        // TODO: Implement animation    

        if (collision.TryGetComponent<Alive>(out var alive))
        {
            entitiesInsideArea.Add(alive);
        }

        if (!collision.CompareTag("Player") || !active) return;

        active = false; 
        OnTrapTriggered?.Invoke();
        Invoke("DoTrapDamage", timeTillDamage);
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