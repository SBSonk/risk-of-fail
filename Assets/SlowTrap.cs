using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTrap : WalkableTrap
{
    [SerializeField] private float speedPenaltyMultiplier = 0.75f;
    
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.TryGetComponent<PlayerMovement>(out var p))
        {
            p.SetSpeedMultiplier(speedPenaltyMultiplier);
        } else if (collision.TryGetComponent<Enemy>(out var e))
        {
            e.SetSpeedMultiplier(speedPenaltyMultiplier);
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        // Speed up whoever
        if (collision.TryGetComponent<PlayerMovement>(out var p))
        {
            p.ResetSpeed();
        } else if (collision.TryGetComponent<Enemy>(out var e))
        {
            e.ResetSpeed();
        }
        
        base.OnTriggerExit2D(collision);
    }
}
