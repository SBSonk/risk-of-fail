using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudManager3 : MonoBehaviour
{
    PlayerStatus player;
    PlayerShooting shooting;

    [Header("Health Bar")]
    [SerializeField] RectTransform healthBarPivot;
    [SerializeField] float healthBarLerp = 0.1f;
    Vector3 healthTargetScale = Vector3.one;

    [Header("Fudge Points")]
    [SerializeField] TextMeshProUGUI fudgePointsText;

    [Header("Ammo")]
    [SerializeField] RectTransform ammoBar;
    [SerializeField] TextMeshProUGUI gunName;
    [SerializeField] float ammoBarLerp = 0.5f;
    [SerializeField] Image weaponSprite;
    [SerializeField] TextMeshProUGUI reserveAmount;
    [SerializeField] Image ammobarImage;
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color reloadingColor = Color.cyan;
    Vector3 ammoTargetScale = Vector3.one;

    [Header("Weapon Stats")]
    [SerializeField] Image strength;
    [SerializeField] Image fireRate;
    [SerializeField] Image piercing;
    [SerializeField] Sprite[] barStates;

    Animator animator;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerStatus>();
        shooting = player.GetComponent<PlayerShooting>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        PlayerStatus.onPlayerDamage += UpdateHealth;
        PlayerStatus.onPlayerHeal += UpdateHealth;
        GameManager.onEnemyKilled += UpdatePoints;
        PlayerShooting.onPlayerShoot += UpdateAmmo;
        PlayerShooting.onReloadStart += StartReloadAnimation;
        PlayerShooting.onAmmoUpdate += UpdateAmmo;
        PlayerShooting.onWeaponSwitch += SwapWeapon;
    }

    private void OnDestroy()
    {
        PlayerStatus.onPlayerDamage -= UpdateHealth;
        PlayerStatus.onPlayerHeal -= UpdateHealth;
        GameManager.onEnemyKilled -= UpdatePoints;
        PlayerShooting.onPlayerShoot -= UpdateAmmo;
        PlayerShooting.onReloadStart -= StartReloadAnimation;
        PlayerShooting.onAmmoUpdate -= UpdateAmmo;
        PlayerShooting.onWeaponSwitch -= SwapWeapon;
    }

    private void FixedUpdate()
    {   
        healthBarPivot.localScale = Vector3.Lerp(healthBarPivot.localScale, healthTargetScale, healthBarLerp);
        ammoBar.localScale = Vector3.Lerp(ammoBar.localScale, ammoTargetScale, ammoBarLerp);
    }

    void UpdateHealth()
    {
        healthTargetScale.x = player.health / player.maxHealth;
    }

    void UpdatePoints()
    {
        fudgePointsText.text = GameManager.main.score.ToString("D5");
    }

    void StartReloadAnimation()
    {
        StartCoroutine(ReloadAnimation());
    }

    void UpdateAmmo()
    {
        var inventoryWep = shooting.GetHeldWeapon();

        if (inventoryWep.weapon.clipSize > 0) ammoTargetScale.x = (float) inventoryWep.clip / inventoryWep.weapon.clipSize;
        else ammoTargetScale.x = 1;

        reserveAmount.text = inventoryWep.pool.ToString();
    }

    void SwapWeapon()
    {
        var currentWep = shooting.GetHeldWeapon().weapon;
        weaponSprite.sprite = currentWep.hudElement;
        weaponSprite.rectTransform.sizeDelta = new Vector2(currentWep.hudElement.rect.width, currentWep.hudElement.rect.height);

        // Update text and weapon stat ui
        gunName.text = currentWep.weaponName;
        strength.sprite = barStates[currentWep.weaponStrength];
        fireRate.sprite = barStates[currentWep.weaponFireRate];
        piercing.sprite = barStates[currentWep.weaponPiercing];

        animator.Play("SwapWeapon", 0, 0);

        UpdateAmmo();
    }

    IEnumerator ReloadAnimation()
    {
        float startTime = Time.time;
        float finishTime = shooting.GetHeldWeapon().weapon.reloadLength;

        // Change ammo bar to reloading color
        ammobarImage.color = reloadingColor;

        ammoTargetScale.x = 0;
        ammoBar.localScale = ammoTargetScale;

        // Update ammo bar to reload state
        float currentTime = Time.time - startTime;
        while (currentTime < finishTime)
        {
            ammoTargetScale.x = currentTime / finishTime;

            yield return new WaitForFixedUpdate();
            currentTime = Time.time - startTime;
        }

        // Revert ammo bar color
        ammobarImage.color = defaultColor;

        UpdateAmmo();
    }
}
