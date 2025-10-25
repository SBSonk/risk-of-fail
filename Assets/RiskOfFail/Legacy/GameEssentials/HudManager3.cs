using System.Collections;
using RiskOfFail.Combat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudManager3 : MonoBehaviour
{
    [Header("Health Bar")] [SerializeField]
    private RectTransform healthBarPivot;

    [SerializeField] private float healthBarLerp = 0.1f;

    [Header("Fudge Points")] [SerializeField]
    private TextMeshProUGUI fudgePointsText;

    [Header("Ammo")] [SerializeField] private RectTransform ammoBar;

    [SerializeField] private TextMeshProUGUI gunName;
    [SerializeField] private float ammoBarLerp = 0.5f;
    [SerializeField] private Image weaponSprite;
    [SerializeField] private TextMeshProUGUI reserveAmount;
    [SerializeField] private Image ammobarImage;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color reloadingColor = Color.cyan;
    [SerializeField] private RectTransform ammoDivisionTransform;
    [SerializeField] private GameObject ammoDivision;

    [Header("Weapon Stats")] [SerializeField]
    private Image strength;

    [SerializeField] private Image fireRate;
    [SerializeField] private Image piercing;
    [SerializeField] private Sprite[] barStates;

    [Header("Dodge")] [SerializeField] private TextMeshProUGUI dodgeText;

    private Vector3 ammoTargetScale = Vector3.one;

    private Animator animator;
    private Vector3 healthTargetScale = Vector3.one;
    private PlayerStatus player;
    private PlayerShooting shooting;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerStatus>();
        shooting = player.GetComponent<PlayerShooting>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //PlayerStatus.onPlayerDamage += UpdateHealth;
        //PlayerStatus.onPlayerHeal += UpdateHealth;
        //GameManager.onEnemyKilled += UpdatePoints;
        //GameManager.onWeaponReceive += SwapWeapon;
        /*PlayerShooting.onPlayerShoot += UpdateAmmo;
        PlayerShooting.onReloadStart += StartReloadAnimation;
        PlayerShooting.onAmmoUpdate += UpdateAmmo;
        PlayerShooting.onWeaponSwitch += SwapWeapon;*/

        //PlayerMovement.onDodge += UpdateDodges;

        // Initialize UI
        UpdatePoints();
        SwapWeapon();
        UpdateDodges();
    }

    private void FixedUpdate()
    {
        healthBarPivot.localScale = Vector3.Lerp(healthBarPivot.localScale, healthTargetScale, healthBarLerp);
        ammoBar.localScale = Vector3.Lerp(ammoBar.localScale, ammoTargetScale, ammoBarLerp);
    }

    private void OnDestroy()
    {
        //PlayerStatus.onPlayerDamage -= UpdateHealth;
        //PlayerStatus.onPlayerHeal -= UpdateHealth;
        //GameManager.onEnemyKilled -= UpdatePoints;
        //GameManager.onWeaponReceive -= SwapWeapon;
        /*PlayerShooting.onPlayerShoot -= UpdateAmmo;
        PlayerShooting.onReloadStart -= StartReloadAnimation;
        PlayerShooting.onAmmoUpdate -= UpdateAmmo;
        PlayerShooting.onWeaponSwitch -= SwapWeapon;*/

        //PlayerMovement.onDodge -= UpdateDodges;
    }

    private void UpdateHealth()
    {
        healthTargetScale.x = player.health / player.maxHealth;
    }

    private void UpdatePoints()
    {
        fudgePointsText.text = GameManager.main.pData.fPoints.ToString("D5");
    }

    private void StartReloadAnimation()
    {
        StartCoroutine(ReloadAnimation());
    }

    private void UpdateAmmo()
    {
        var inventoryWep = shooting.GetHeldWeapon();

        if (inventoryWep.weapon.clipSize > 0)
            ammoTargetScale.x = (float)inventoryWep.clip / inventoryWep.weapon.clipSize;
        else ammoTargetScale.x = 1;

        reserveAmount.text = inventoryWep.pool.ToString();
    }

    private void SwapWeapon()
    {
        // Cancel reload animation, if started
        StopAllCoroutines();

        var currentWep = shooting.GetHeldWeapon().weapon;
        weaponSprite.sprite = currentWep.hud.sprite;
        weaponSprite.rectTransform.sizeDelta =
            new Vector2(currentWep.hud.sprite.rect.width, currentWep.hud.sprite.rect.height);

        // Update text and weapon stat ui
        gunName.text = currentWep.name;
        strength.sprite = barStates[currentWep.shopData.weaponStrength];
        fireRate.sprite = barStates[currentWep.shopData.weaponFireRate];
        piercing.sprite = barStates[currentWep.shopData.weaponPiercing];

        animator.Play("SwapWeapon", 0, 0);

        // Clear ammo division children
        if (ammoDivisionTransform.childCount > 0)
        {
            var children = ammoDivisionTransform.GetComponentsInChildren<RectTransform>();
            for (var i = 1; i < children.Length; i++) Destroy(children[i].gameObject);
        }

        var distance = ammoDivisionTransform.sizeDelta.x;
        distance /= currentWep.clipSize;

        // Remake ammo divisions
        for (var i = 1; i < currentWep.clipSize; i++)
        {
            var trans = Instantiate(ammoDivision, ammoDivisionTransform).GetComponent<RectTransform>();
            trans.position = ammoDivisionTransform.position;
            trans.localPosition += Vector3.right * i * distance;
        }

        UpdateAmmo();
    }

    private IEnumerator ReloadAnimation()
    {
        var startTime = Time.time;
        var finishTime = shooting.GetHeldWeapon().weapon.reloadLength;

        // Change ammo bar to reloading color
        ammobarImage.color = reloadingColor;

        ammoTargetScale.x = 0;
        ammoBar.localScale = ammoTargetScale;

        // Update ammo bar to reload state
        var currentTime = Time.time - startTime;
        while (currentTime < finishTime)
        {
            ammoTargetScale.x = currentTime / finishTime;

            yield return new WaitForEndOfFrame();
            currentTime = Time.time - startTime;
        }

        // Revert ammo bar color
        ammobarImage.color = defaultColor;

        UpdateAmmo();
    }

    public void UpdateDodges()
    {
        //dodgeText.text = $"DODGES: {player.pMovement.dodges}";
    }
}