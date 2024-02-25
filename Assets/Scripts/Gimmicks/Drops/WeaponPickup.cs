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

    protected override void PlayerInteract()
    {
        PlayerShooting shooting = PlayerStatus.player.pShooting;

        // Check if weapon owned
        if (!shooting.CheckIfWeaponOwned(weaponToGive))
        {
            if (shooting.InventoryFull()) shooting.ReplaceWeapon(inventoryWeapon, shooting.GetHeldIndex());
            else shooting.GiveWeapon(inventoryWeapon);
        }
        else
        {
            shooting.GiveAmmo(shooting.GetWeaponFromInventory(weaponToGive), Mathf.FloorToInt(weaponToGive.defaultAmmoCount/4f));
        }

        Destroy(gameObject);
    }

    public void SetWeapon(InventoryWeapon w)
    {
        weaponToGive = w.weapon;
        inventoryWeapon = w;
        
        UpdateSprite();
    }

    void UpdateSprite() => sprite.sprite = weaponToGive.weaponSprite;
}
