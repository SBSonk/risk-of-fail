using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : PickupBase
{
    [Header("Pickup Stats")]
    [SerializeField] int amount = 10;
    [SerializeField] int minAmount = 1;

    protected override void OnTriggerStay2D(Collider2D other)
    {
        if (!active) return;

        // Return if the player isnt the one who collected
        if (!other.CompareTag("Player")) return;

        var player = other.GetComponent<Alive>();
        if (player.health < player.maxHealth)
        {
            int finalAmount = Random.Range(minAmount, amount);

            player.GiveHealth(finalAmount);
        }

        PlayPickupAnimation();
        active = false;
    }
}
