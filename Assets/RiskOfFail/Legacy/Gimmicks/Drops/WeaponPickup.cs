using RiskOfFail.Combat;
using UnityEngine;

public class WeaponPickup : InteractBase
{
    public Weapon weaponToGive;
    public InventoryWeapon inventoryWeapon;

    [SerializeField] private SpriteRenderer sprite;

    protected override void Start()
    {
        base.Start();

        UpdateSprite();
        inventoryWeapon = new InventoryWeapon(weaponToGive, weaponToGive.clipSize, weaponToGive.defaultAmmoCount);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        base.OnTriggerEnter2D(collision);

        PlayerStatus.instance.shooting.weaponsInArea.Add(this);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        base.OnTriggerExit2D(collision);

        PlayerStatus.instance.shooting.weaponsInArea.Remove(this);
    }

    protected override void PlayerInteract()
    {
        // empty to move logic to player
    }

    public void PickupWeapon()
    {
        OnPickup?.Invoke();

        // Save Weapon
        WeaponSelector.SetWeaponOwned(weaponToGive.name);

        PlayerShooting shooting = PlayerStatus.instance.shooting;

        // Check if weapon owned
        if (!shooting.CheckIfWeaponOwned(weaponToGive))
        {
            if (shooting.InventoryFull()) shooting.ReplaceWeapon(inventoryWeapon, shooting.GetHeldIndex());
            else shooting.GiveWeapon(inventoryWeapon);
        }
        else
        {
            shooting.GiveAmmo(shooting.GetWeaponFromInventory(weaponToGive),
                Mathf.FloorToInt(weaponToGive.defaultAmmoCount / 4f));
        }

        PlayerStatus.instance.shooting.weaponsInArea.Remove(this);
        Destroy(gameObject);
    }

    public void SetWeapon(InventoryWeapon w)
    {
        weaponToGive = w.weapon;
        inventoryWeapon = w;

        UpdateSprite();
    }

    private void UpdateSprite()
    {
        sprite.sprite = weaponToGive.weaponSprite;
    }
}