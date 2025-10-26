using System.Collections;
using System.Collections.Generic;
using RiskOfFail.Combat;
using UnityEngine;

public class FloatyDock : InteractBase
{
    public Transform spawnPoint;

    private List<RubberDucky> duckies = new List<RubberDucky>();

    protected override void PlayerInteract()
    {
        // check if on floaty
        foreach (var duck in duckies)
        {
            if (duck.riding)
            {
                duck.riding = false;
                PlayerStatus.instance.transform.position = spawnPoint.position;
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out RubberDucky e)) // THIS IS HORRIBLE BRAIN NO WORKY
        {
            return;
        }
        playerInRadius = true;
        playerTransform = collision.transform;

        OnPlayerEnter?.Invoke();
        
        if (anim)
            anim.Play("InRange");

        if (collision.TryGetComponent(out RubberDucky d))
        {
            duckies.Add(d);
            d.canExit = true;
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out RubberDucky e))
        {
            return;
        }
        playerInRadius = false;
        playerTransform = null;

        OnPlayerLeave?.Invoke();
        
        if (anim)
            anim.Play("OutRange");

        if (collision.TryGetComponent(out RubberDucky d))
        {
            duckies.Remove(d);
            d.canExit = false;
        }
    }
}
