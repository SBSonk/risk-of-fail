using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using Pathfinding.Util;

public class HudManager4 : MonoBehaviour
{
    public PlayerStatus player;

    [SerializeField] float barLerp = 0.25f;

    [SerializeField] Image avatarImage;
    [SerializeField] AvatarState[] AvatarImages;

    [SerializeField] Image healthBar;

    [SerializeField] float AMMOPADDING = 20, AMMOAPPEARTIME = .1f;
    [SerializeField] GameObject ammoPrefab;
    [SerializeField] Transform ammoParent, ammoBar;
    [SerializeField] TextMeshProUGUI poolCount;
    [SerializeField] Image weaponSprite;
    [SerializeField] RectTransform weaponTransform;
    Coroutine ammoAnimation;

    [SerializeField] TextMeshProUGUI fudgePointsText;

    [SerializeField] Image dodgeBar;

    [SerializeField] Animator weaponAnim, avatarAnim;
    Vector3 defaultAvatarPos;

    // Handle Player Avatar
    void PlayerAvatarAnimation(float amount)
    {
        // Update player avatar to damage state
        // Choose image to display
        for (int i = 0; i < AvatarImages.Length; i++)
        {
            if (player.health > AvatarImages[i].healthGreaterThan)
            {
                // Display image
                avatarImage.sprite = AvatarImages[i].image;
            }
        }

        //avatarAnim.CrossFade("AvatarJump", 0.1f, 0, 0);
    }

    // Handle Weapon Swap
    void UpdateWeaponIcon()
    {
        var weapon = player.pShooting.GetHeldWeapon();

        // Swap sprite
        weaponSprite.sprite = weapon.weapon.hud.sprite;
        weaponTransform.localPosition = weapon.weapon.hud.offset;
        weaponTransform.sizeDelta = weaponSprite.sprite.rect.size;
        weaponAnim.Play("SwapWeapon2", 0, 0);

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
            if (ammoAnimation != null) StopCoroutine(ammoAnimation);
            ammoAnimation = StartCoroutine(SpawnAmmo(ammoParent.childCount, cCount, AMMOAPPEARTIME));
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

    IEnumerator SpawnAmmo(int childCount, int ammoCount, float timePerBullet)
    {
        // Add Ammo
        for (int i = childCount; i < ammoCount; i++)
        {
            Instantiate(ammoPrefab, ammoParent).GetComponent<RectTransform>().localPosition =
                new Vector3(i * AMMOPADDING, 0, 0);

            yield return new WaitForSeconds(timePerBullet);
        }
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

    void SetFudgePoints()
    {
        fudgePointsText.SetText(LevelStats.main.points.ToString("00000"));
    }

    private void Start()
    {
        var shooting = player.pShooting;
        shooting.OnWeaponSwitch.AddListener(UpdateWeaponIcon);
        shooting.OnAmmoUpdate.AddListener(UpdateAmmoDisplay);
        shooting.OnWeaponSwitch.AddListener(UpdateAmmoDisplay);

        player.onHeal.AddListener(PlayerAvatarAnimation);
        player.onHit.AddListener(PlayerAvatarAnimation);

        UpdateWeaponIcon();

        defaultAvatarPos = avatarImage.GetComponent<RectTransform>().localPosition;
    }

    private void Update()
    {
        UpdateHealth();
        UpdateDodge();
        SetFudgePoints();
    }
}

[System.Serializable]
struct AvatarState
{
    public int healthGreaterThan;
    public Sprite image;
}