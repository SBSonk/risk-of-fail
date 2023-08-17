using UnityEngine;

public class AmmoPickup : PickupBase
{
    public AmmoDrops drop;

    protected override void Start()
    {
        if (GameManager.main.pData.weaponsOwned.Count == 1)
        {
            Destroy(transform.parent.gameObject);
            return;
        }

        try
        {
            GetComponent<ChooseAmmoDropType>().InitializeAmmo();
            
            if (particles) particles.startColor = drop.backgroundColor;
            _light.color = drop.backgroundColor;

            SpriteRenderer spr = sprite.GetComponent<SpriteRenderer>();
            spr.sprite = drop.sprite;
            spr.color = drop.spriteColor;

            base.Start();
        }
        catch
        {
            Destroy(transform.parent.gameObject);
        }
    }

    protected override void OnTriggerStay2D(Collider2D other)
    {
        if (!active) return;

        // Return if the player isnt the one who collected
        if (!other.CompareTag("Player")) return;

        var player = other.GetComponent<PlayerShooting>();
        int ammoToAdd = Random.Range(drop.min, drop.max);

        ammoToAdd = Mathf.RoundToInt((float) ammoToAdd / drop.typeToGive.ammoPerShot) * drop.typeToGive.ammoPerShot;
        
        player.GiveAmmo(player.GetWeaponFromInventory(drop.typeToGive), ammoToAdd);

        PlayPickupAnimation();
        active = false;
    }
}