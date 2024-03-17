using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KillArea : MonoBehaviour
{
    public UnityEvent OnKill;   
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out Enemy e))
        {
            e.onEnemyDeath.AddListener(KillEvent);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy e))
        {
            e.onEnemyDeath.RemoveListener(KillEvent);
        }
    }

    void KillEvent(EnemyType _) => OnKill?.Invoke();
}
