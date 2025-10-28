using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class LockedDoor : Door
{
    [CanBeNull] public UnityEvent OnDoorUnlock;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        
        // Check if key is in area  
        Collider2D[] c = Physics2D.OverlapCircleAll(col.transform.position, 10f);
        foreach (var collider in c)
        {
            if (collider.TryGetComponent<Key>(out var key))
            {
                key.TryOpenDoor(this);
            }
        }
    }

    public override void ToggleDoor(bool val)
    {
        base.ToggleDoor(val);
        
        if (val) OnDoorUnlock?.Invoke();
    }
}
