using RiskOfFail.Combat;
using RiskOfFail.Combat.Enums;
using UnityEngine;

public class HealthPickup : PickupBase
{
    [Header("Pickup Stats")] [SerializeField]
    private int amount = 10;

    [SerializeField] private int minAmount = 1;

    protected override void OnTriggerStay2D(Collider2D other)
    {
        if (!active) return;

        // Return if the player isnt the one who collected
        if (!other.CompareTag("Player")) return;

        var player = other.GetComponent<Alive>();
        if (player.health < player.maxHealth)
        {
            var finalAmount = Random.Range(minAmount, amount);

            player.GiveHealth(finalAmount, DamageTypeFlag.Self);
        }

        PlayPickupAnimation();
        active = false;
    }
}