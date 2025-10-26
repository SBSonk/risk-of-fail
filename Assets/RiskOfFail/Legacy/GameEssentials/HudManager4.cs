using System;
using System.Collections;
using RiskOfFail.Combat;
using RiskOfFail.Combat.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HudManager4 : MonoBehaviour
{
    public static HudManager4 hud;

    [SerializeField] private float barLerp = 0.25f;

    [SerializeField] private Image avatarImage;
    public Sprite hurtSprite;
    [SerializeField] private AvatarState[] AvatarImages;
    public float avatarHurtTime = 0.5f;

    [SerializeField] private Image healthBar;

    [SerializeField] private float AMMOPADDING = 20, AMMOAPPEARTIME = .1f;
    [SerializeField] private GameObject ammoPrefab;
    [SerializeField] private Transform ammoParent, ammoBar;
    [SerializeField] private TextMeshProUGUI poolCount;
    [SerializeField] private Image weaponSprite;
    [SerializeField] private RectTransform weaponTransform;

    [SerializeField] private TextMeshProUGUI fudgePointsText;

    [SerializeField] private Image dodgeBar;

    [SerializeField] private Animator weaponAnim, avatarAnim;
    private Coroutine ammoAnimation;
    private PlayerStatus player;

    private void Awake()
    {
        hud = this;
    }

    private void Update()
    {
        if (!player) return;

        UpdateHealth();
        UpdateDodge();
        SetFudgePoints();
    }

    // Handle Player Avatar
    private void PlayerAvatarAnimation(float amount, DamageTypeFlag _)
    {
        // Choose image to display
        for (var i = 0; i < AvatarImages.Length; i++)
            if (player.health > player.maxHealth * AvatarImages[i].healthGreaterThanRatio)
                // Display image
                avatarImage.sprite = AvatarImages[i].image;
    }

    private void PlayerAvatarJump(float _, DamageTypeFlag __)
    {
        avatarAnim.CrossFade("AvatarJump", 0.1f, 0, 0);

        StopCoroutine(nameof(AvatarHurtAnimation));
        StartCoroutine(AvatarHurtAnimation());
    }

    private IEnumerator AvatarHurtAnimation()
    {
        var lastSprite = avatarImage.sprite;
        avatarImage.sprite = hurtSprite;

        yield return new WaitForSeconds(avatarHurtTime);

        avatarImage.sprite = lastSprite;
    }

    // Handle Weapon Swap
    public void UpdateWeaponIcon(InventoryWeapon w)
    {
        // Swap sprite
        weaponSprite.sprite = w.weapon.hud.sprite;
        weaponTransform.localPosition = w.weapon.hud.offset;
        weaponTransform.sizeDelta = weaponSprite.sprite.rect.size;
        weaponAnim.Play("SwapWeapon2", 0, 0);

        // Hide ammo if melee
        ammoBar.gameObject.SetActive(!(w.weapon is MeleeWeapon));

        UpdateAmmoDisplay(w);
    }

    // Update Ammo Display
    private void UpdateAmmoDisplay(InventoryWeapon weapon)
    {
        var pCount = weapon.pool;
        var cCount = weapon.clip;

        if (cCount < 0) return;

        if (ammoParent.childCount < cCount)
        {
            if (ammoAnimation != null) StopCoroutine(ammoAnimation);
            ammoAnimation = StartCoroutine(SpawnAmmo(ammoParent.childCount, cCount, AMMOAPPEARTIME));
        }
        else
        {
            // Subtract Ammo
            for (var i = ammoParent.childCount; i > cCount; i--) Destroy(ammoParent.GetChild(i - 1).gameObject);
        }

        poolCount.SetText(pCount.ToString());
    }

    private IEnumerator SpawnAmmo(int childCount, int ammoCount, float timePerBullet)
    {
        // Add Ammo
        for (var i = childCount; i < ammoCount; i++)
        {
            Instantiate(ammoPrefab, ammoParent).GetComponent<RectTransform>().localPosition =
                new Vector3(i * AMMOPADDING, 0, 0);

            yield return new WaitForSeconds(timePerBullet);
        }
    }

    private void UpdateHealth()
    {
        if (PlayerStatus.instance)
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, player.health / player.maxHealth, barLerp);
        else
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, 0, barLerp);
    }

    // Handle Dodge Cooldown
    private void UpdateDodge()
    {
        /*dodgeBar.fillAmount = Mathf.Lerp(dodgeBar.fillAmount, player.pMovement.dodges / player.pMovement.maxDodges,
            barLerp);*/
    }

    private void SetFudgePoints()
    {
        fudgePointsText.SetText(LevelStats.main.points.ToString("00000"));
    }

    public void SetPlayer(PlayerStatus _player)
    {
        player = _player;

        var shooting = player.shooting;
        shooting.OnWeaponSwitch.AddListener(UpdateWeaponIcon);
        shooting.OnAmmoUpdate.AddListener(UpdateAmmoDisplay);
        shooting.OnWeaponSwitch.AddListener(UpdateAmmoDisplay);

        /*player.onHeal.AddListener(PlayerAvatarAnimation);
        player.onHit.AddListener(PlayerAvatarAnimation);
        player.onHit.AddListener(PlayerAvatarJump);*/

        UpdateWeaponIcon(shooting.GetHeldWeapon());
    }
}

[Serializable]
internal struct AvatarState
{
    [FormerlySerializedAs("healthGreaterThan")]
    public float healthGreaterThanRatio;

    public Sprite image;
}