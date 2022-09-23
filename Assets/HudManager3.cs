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
    Vector3 ammoTargetScale = Vector3.one;

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
        PlayerShooting.onAmmoUpdate += UpdateAmmo;
        PlayerShooting.onWeaponSwitch += SwapWeapon;
    }

    private void OnDestroy()
    {
        PlayerStatus.onPlayerDamage -= UpdateHealth;
        PlayerStatus.onPlayerHeal -= UpdateHealth;
        GameManager.onEnemyKilled -= UpdatePoints;
        PlayerShooting.onPlayerShoot -= UpdateAmmo;
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

    void UpdateAmmo()
    {
        inventoryWeapon current = shooting.weaponPool[shooting.currentWeaponIndex];

        if (current.weapon.clipSize > 0) ammoTargetScale.x = (float) current.clip / current.weapon.clipSize;
        else ammoTargetScale.x = 1;

        reserveAmount.text = current.pool.ToString();
    }

    void SwapWeapon()
    {
        var weapon = shooting.weaponPool[shooting.currentWeaponIndex].weapon;
        weaponSprite.sprite = weapon.hudElement;
        weaponSprite.rectTransform.sizeDelta = new Vector2(weapon.hudElement.rect.width, weapon.hudElement.rect.height);

        gunName.text = weapon.weaponName;

        animator.Play("SwapWeapon", 0, 0);

        UpdateAmmo();
    }
}
