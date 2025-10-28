using System;
using System.Collections;
using System.Collections.Generic;
using RiskOfFail.Combat;
using UnityEngine;

public class SlowTrap : WalkableTrap
{
    [SerializeField] private float speedPenaltyMultiplier = 0.75f;

    private List<Enemy> enemiesInside = new List<Enemy>();
    private List<PlayerMovement> playersInside = new List<PlayerMovement>();

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.TryGetComponent<PlayerMovement>(out var p))
        {
            if (!playersInside.Contains(p))
            {
                playersInside.Add(p);
                p.SetSpeedMultiplier(speedPenaltyMultiplier);
            }
        } else if (collision.TryGetComponent<Enemy>(out var e))
        {
            if (!enemiesInside.Contains(e))
            {
                enemiesInside.Add(e);
                //e.SetSpeedMultiplier(speedPenaltyMultiplier);
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        // Speed up whoever
        if (collision.TryGetComponent<PlayerMovement>(out var p))
        {
            p.ResetSpeed();
            playersInside.Remove(p);
        } else if (collision.TryGetComponent<Enemy>(out var e))
        {
            //e.ResetSpeed();
            enemiesInside.Remove(e);
        }
        
        base.OnTriggerExit2D(collision);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < playersInside.Count; i++)
        {
            playersInside[i].ResetSpeed();
        }

        for (int i = 0; i < enemiesInside.Count; i++)
        {
            //enemiesInside[i].ResetSpeed();
        }
    }

    // todo: edge case wherein the object is destroyed when entity is being slowed, entities remain slowed
}
