using UnityEngine;

public class AmmoPickup : PickupBase
{
    public AmmoDrops drop;

    private void Start()
    {
        particles.startColor = drop.backgroundColor;
        _light.color = drop.backgroundColor;
        sprite.GetComponent<SpriteRenderer>().color = drop.spriteColor;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        // Return if the player isnt the one who collected
        if (!other.CompareTag("Player")) return;

        var player = other.GetComponent<PlayerShooting>();
        int ammoToAdd = Random.Range(drop.min, drop.max);

        player.GetWeaponFromInventory(drop.typeToGive).GiveAmmo(ammoToAdd);

        PlayPickupAnimation();
    }
}