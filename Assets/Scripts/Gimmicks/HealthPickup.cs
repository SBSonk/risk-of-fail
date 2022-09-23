using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : PickupBase
{
    [Header("Pickup Stats")]
    [SerializeField] int amount = 10;
    [SerializeField] int minAmount = 1;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        // Return if the player isnt the one who collected
        if (!other.CompareTag("Player")) return;

        // Decide how much ammo or health to give
        int finalAmount = Random.Range(minAmount, amount);

        other.GetComponent<Alive>().GiveHealth(finalAmount);

        PlayPickupAnimation();
    }
}
