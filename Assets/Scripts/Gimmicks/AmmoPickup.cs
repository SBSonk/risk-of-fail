using UnityEngine;

public class AmmoPickup : PickupBase
{
    public AmmoDrops drop;

    private void Start()
    {
        particles.startColor = drop.backgroundColor;
        _light.color = drop.backgroundColor;

        SpriteRenderer spr = sprite.GetComponent<SpriteRenderer>();
        spr.sprite = drop.sprite;
        spr.color = drop.spriteColor;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!active) return;

        // Return if the player isnt the one who collected
        if (!other.CompareTag("Player")) return;

        var player = other.GetComponent<PlayerShooting>();
        int ammoToAdd = Random.Range(drop.min, drop.max);

        player.GiveAmmo(player.GetWeaponFromInventory(drop.typeToGive), ammoToAdd);

        PlayPickupAnimation();
        active = false;
    }
}