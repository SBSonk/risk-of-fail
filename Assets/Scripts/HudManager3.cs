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
    [SerializeField] RectTransform ammoDivisionTransform;
    [SerializeField] GameObject ammoDivision;

    [Header("Weapon Stats")]
    [SerializeField] Image strength;
    [SerializeField] Image fireRate;
    [SerializeField] Image piercing;
    [SerializeField] Sprite[] barStates;

    [Header("Dodge")]
    [SerializeField] TextMeshProUGUI dodgeText;

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
        //GameManager.onEnemyKilled += UpdatePoints;
        GameManager.onWeaponReceive += SwapWeapon;
        /*PlayerShooting.onPlayerShoot += UpdateAmmo;
        PlayerShooting.onReloadStart += StartReloadAnimation;
        PlayerShooting.onAmmoUpdate += UpdateAmmo;
        PlayerShooting.onWeaponSwitch += SwapWeapon;*/

        PlayerMovement.onDodge += UpdateDodges;

        // Initialize UI
        UpdatePoints();
        SwapWeapon();
        UpdateDodges();
    }

    private void OnDestroy()
    {
        PlayerStatus.onPlayerDamage -= UpdateHealth;
        PlayerStatus.onPlayerHeal -= UpdateHealth;
        //GameManager.onEnemyKilled -= UpdatePoints;
        GameManager.onWeaponReceive -= SwapWeapon;
        /*PlayerShooting.onPlayerShoot -= UpdateAmmo;
        PlayerShooting.onReloadStart -= StartReloadAnimation;
        PlayerShooting.onAmmoUpdate -= UpdateAmmo;
        PlayerShooting.onWeaponSwitch -= SwapWeapon;*/

        PlayerMovement.onDodge -= UpdateDodges;
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
        fudgePointsText.text = GameManager.main.pData.fPoints.ToString("D5");
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
        // Cancel reload animation, if started
        StopAllCoroutines();

        var currentWep = shooting.GetHeldWeapon().weapon;
        weaponSprite.sprite = currentWep.hud.sprite;
        weaponSprite.rectTransform.sizeDelta = new Vector2(currentWep.hud.sprite.rect.width, currentWep.hud.sprite.rect.height);

        // Update text and weapon stat ui
        gunName.text = currentWep.name;
        strength.sprite = barStates[currentWep.shopData.weaponStrength];
        fireRate.sprite = barStates[currentWep.shopData.weaponFireRate];
        piercing.sprite = barStates[currentWep.shopData.weaponPiercing];

        animator.Play("SwapWeapon", 0, 0);

        // Clear ammo division children
        if (ammoDivisionTransform.childCount > 0)
        {
            RectTransform[] children = ammoDivisionTransform.GetComponentsInChildren<RectTransform>();
            for (int i = 1; i < children.Length; i++)
            {
                Destroy(children[i].gameObject);
            }
        }

        float distance = ammoDivisionTransform.sizeDelta.x;
        distance /= currentWep.clipSize;
            
        // Remake ammo divisions
        for (int i = 1; i < currentWep.clipSize; i++)
        {
            var trans = Instantiate(ammoDivision, ammoDivisionTransform).GetComponent<RectTransform>();
            trans.position = ammoDivisionTransform.position;
            trans.localPosition += Vector3.right * i * distance;
        }

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

            yield return new WaitForEndOfFrame();
            currentTime = Time.time - startTime;
        }

        // Revert ammo bar color
        ammobarImage.color = defaultColor;

        UpdateAmmo();
    }

    public void UpdateDodges()
    {
        dodgeText.text = $"DODGES: {player.pMovement.dodges}";
    }
}
