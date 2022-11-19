using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class HudManager4 : MonoBehaviour
{
    public PlayerStatus player;

    [SerializeField] float barLerp = 0.25f;
    
    [SerializeField] Image healthBar;

    [SerializeField] float AMMOPADDING = 20;
    [SerializeField] GameObject ammoPrefab;
    [SerializeField] Transform ammoParent, ammoBar;
    [SerializeField] TextMeshProUGUI poolCount;
    [SerializeField] Image weaponSprite;
    [SerializeField] RectTransform weaponTransform;

    [SerializeField] Image dodgeBar;

    // Handle Player Avatar

    // Handle Weapon Swap
    void UpdateWeaponIcon()
    {
        var weapon = player.pShooting.GetHeldWeapon();

        // Swap sprite
        weaponSprite.sprite = weapon.weapon.hud.sprite;
        weaponTransform.sizeDelta = weaponSprite.sprite.rect.size;
        weaponTransform.localPosition = weapon.weapon.hud.offset;

        // Hide ammo if melee
        ammoBar.gameObject.SetActive(!(weapon.weapon is MeleeWeapon));

        UpdateAmmoDisplay();
    }

    // Update Ammo Display
    void UpdateAmmoDisplay()
    {
        var weapon = player.pShooting.GetHeldWeapon();
        int pCount = weapon.pool;
        int cCount = weapon.clip;

        if (cCount < 0) return;

        if (ammoParent.childCount < cCount)
        {
            // Add Ammo
            for (int i = ammoParent.childCount; i < cCount; i++)
            {
                Instantiate(ammoPrefab, ammoParent).GetComponent<RectTransform>().localPosition =
                    new Vector3(i * AMMOPADDING, 0, 0);
            }
        } else
        {
            // Subtract Ammo
            for (int i = ammoParent.childCount; i > cCount; i--)
            {
                Destroy(ammoParent.GetChild(i-1).gameObject);
            }
        }

        poolCount.SetText(pCount.ToString());
    }

    void UpdateHealth()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, player.health / player.maxHealth, barLerp);
    }

    // Handle Dodge Cooldown
    void UpdateDodge()
    {
        dodgeBar.fillAmount = Mathf.Lerp(dodgeBar.fillAmount, player.pMovement.dodges / player.pMovement.maxDodges, barLerp);
    }

    private void Start()
    {
        var shooting = player.pShooting;
        shooting.OnWeaponSwitch.AddListener(UpdateWeaponIcon);
        shooting.OnAmmoUpdate.AddListener(UpdateAmmoDisplay);
        shooting.OnWeaponSwitch.AddListener(UpdateAmmoDisplay);

        UpdateWeaponIcon();
    }

    private void Update()
    {
        UpdateHealth();
        UpdateDodge();
    }
}
