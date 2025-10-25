using System.Collections;
using FirstGearGames.SmoothCameraShaker;
using RiskOfFail.Combat;
using RiskOfFail.Combat.Effects;
using RiskOfFail.Combat.Enums;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAnimations : MonoBehaviour
{
    public bool followCursor;
    public bool canSwitchAnimation = true;
    [SerializeField] private SpriteRenderer[] sprites;

    [FormerlySerializedAs("dashExp")] [SerializeField]
    private ParticleSystem dashExplosion;

    [SerializeField] private Animator animator;

    [Header("Dodge")] [SerializeField] private TrailRenderer trail;

    [SerializeField] private float trailLifetime = 0.5f;
    [SerializeField] private ShakeData dodgeScreenshake;

    [Header("Player Sprite")] [SerializeField]
    private SpriteRenderer sprite;

    [Header("Weapon")] [SerializeField] private Transform weaponPivot;
    [SerializeField] private WeaponAnimator weaponAnimator;
    [SerializeField] private float weaponLerp = 0.5f;
    public ShakeData shoveShake;

    private Directions currentDir = Directions.down;
    private Vector2 input;
    private PlayerShooting pShooting;

    private Rigidbody2D rb;
    private float trailTime;

    private void Update()
    {
        if (PauseMenu.paused) return;

        GetInputs();
    }

    private void FixedUpdate()
    {
        if (!canSwitchAnimation || PauseMenu.paused) return;

        if (followCursor || input.magnitude > 0.25f)
        {
            OrientPlayer();
            OrientWeapon(weaponLerp);
        }

        // Choose animations
        if (input.magnitude > 0.25f)
            PlayDirectionalAnimation("walk", currentDir);
        else
            PlayDirectionalAnimation("stand", currentDir);
    }

    public void Initialize(PlayerShooting shooting, PlayerMovement movement, PlayerStatus status)
    {
        shooting.OnShoot.AddListener(PlayShootAnimation);
        shooting.OnWeaponSwitch.AddListener(ChangeWeaponSprite);
        shooting.OnShove.AddListener(PlayShoveAnimation);
        shooting.OnMelee.AddListener(PlayShootAnimation);
        shooting.OnReloadStart.AddListener(PlayReloadAnimation);

        movement.OnDodge.AddListener(PlayDodgeAnimation);

        status.onHit.AddListener(PlayDamageAnimation);
        status.onDeath.AddListener(PlayDeathAnimation);

        ChangeWeaponSprite(shooting.GetHeldWeapon());

        trailTime = trail.time;
        trail.time = 0;

        rb = GetComponent<Rigidbody2D>();
        pShooting = shooting;
    }

    private void GetInputs()
    {
        var playerDirection = new Vector2(KeyBind.GetAxis(KInputManager.GetKey("Right"),
            KInputManager.GetKey("Left")), KeyBind.GetAxis(KInputManager.GetKey("Up"), KInputManager.GetKey("Down")));
        input = playerDirection;
    }

    private void DetermineAimMode(Weapon w)
    {
        followCursor = w is Gun;
        MoveCursor.instance.SetVisibility(followCursor);
    }

    private void OrientPlayer()
    {
        Vector2 dir = followCursor ? MoveCursor.instance.GetMousePlayerDirection() : input;

        currentDir = HelperFunctions.VectorToDir(dir);
    }

    private void OrientWeapon(float lerp)
    {
        if (input.magnitude < 0.25f && !followCursor) return;

        var angle = HelperFunctions.VectorToAngle(followCursor ? MoveCursor.instance.GetMousePlayerDirection() : input);

        var targetRotation = new Vector3(0, 0, angle);
        weaponAnimator.transform.rotation = Quaternion.Euler(new Vector3(0, 0,
            Mathf.LerpAngle(weaponAnimator.transform.rotation.eulerAngles.z, targetRotation.z, lerp)));

        weaponPivot.rotation = Quaternion.Euler(targetRotation);

        // Adjust Sorting Order
        if (weaponAnimator.sprite) weaponAnimator.sprite.sortingOrder = currentDir == Directions.down ? 1 : 0;
    }

    private string BuildAnimationClipName(string[] args)
    {
        return string.Join("_", args);
    }

    private void PlayDirectionalAnimation(string baseClipName, Directions direction)
    {
        sprite.flipX = direction == Directions.left;

        // force right since they use the same animation
        if (direction == Directions.left) direction = Directions.right;

        var directionSuffix = direction.ToString()[0].ToString();

        animator.Play(BuildAnimationClipName(new[] { baseClipName, directionSuffix }));
    }

    private void PlayDeathAnimation(DamageTypeFlag deathType)
    {
        StopAllCoroutines();
        sprite.color = Color.white;

        StartCoroutine(DeathAnim(deathType));
    }

    private IEnumerator DeathAnim(DamageTypeFlag deathType)
    {
        canSwitchAnimation = false;
        sprite.sortingOrder = 100;
        animator.Play("player_death");

        GetComponent<Collider2D>().enabled = false;

        rb.linearDamping = 0.01f;
        rb.gravityScale = 3;
        rb.AddForce(new Vector3(Mathf.Sign(rb.linearVelocity.x) * 10, 20), ForceMode2D.Impulse);

        if (weaponAnimator)
        {
            weaponAnimator.gameObject.SetActive(false);

            var weaponRb = Instantiate(PlayerStatus.player.pShooting.GetHeldWeapon().weapon.rigidbodyVariant,
                transform.position, transform.rotation);
            weaponRb.AddForce(new Vector3(-Mathf.Sign(rb.linearVelocity.x) * 10, 20), ForceMode2D.Impulse);
            weaponRb.AddTorque(-Mathf.Sign(rb.linearVelocity.x) * 10f, ForceMode2D.Impulse);
        }

        float t = 0;
        while (t < 0.25f)
        {
            Time.timeScale = Mathf.Lerp(1, .5f, t / .25f);
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
        }

        Time.timeScale = 0.5f;

        yield return new WaitForSeconds(.5f);

        t = 0;
        while (t < 0.25f)
        {
            Time.timeScale = Mathf.Lerp(.5f, 1, t / .25f);
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
        }

        Time.timeScale = 1;

        ResultsScreen.instance.ShowResults(deathType);
    }

    private void PlayShootAnimation()
    {
        weaponAnimator.PlayShootAnimation();
    }

    private void PlayReloadAnimation(float _)
    {
        weaponAnimator.PlayReloadAnimation();
    }

    private void PlayShoveAnimation()
    {
        weaponAnimator.PlayShoveAnimation();

        StartCoroutine(ShoveShake(.1f));
    }

    private IEnumerator ShoveShake(float t)
    {
        yield return new WaitForSeconds(t);
        if (shoveShake) CameraShakerHandler.Shake(shoveShake);
    }

    public void ChangeWeaponSprite(InventoryWeapon w)
    {
        MoveCursor.instance.ChangeCrosshair(w.weapon.hud.crossHair);

        // Replace weapon object
        Destroy(weaponAnimator.gameObject);
        weaponAnimator = Instantiate(w.weapon.animatorController,
            transform.position + new Vector3(0, 1.25f) + w.weapon.weaponOffset, Quaternion.identity, sprite.transform);
        weaponAnimator.transform.localScale = w.weapon.weaponScale;

        OrientWeapon(1);

        DetermineAimMode(w.weapon);
    }

    private void PlayDamageAnimation(float _, float ___, DamageTypeFlag __)
    {
        canSwitchAnimation = false;

        PlayDirectionalAnimation("hurt", currentDir);

        Invoke("EnableAnimations", 0.5f);

        StartCoroutine(HelperFunctions.Flicker(sprite, 1, () => sprite.color = Color.white));
    }

    private void PlayDodgeAnimation()
    {
        PlayDirectionalAnimation("dash", currentDir);

        dashExplosion.Play();
        StopCoroutine("DodgeTrail");
        StartCoroutine(DodgeTrail());

        CameraShakerHandler.Shake(dodgeScreenshake);

        OrientWeapon(.75f);

        canSwitchAnimation = false;
        CancelInvoke();
        Invoke("EnableAnimations", 0.3f);
    }

    private IEnumerator DodgeTrail()
    {
        // Enable trail
        trail.time = trailTime;
        trail.emitting = true;

        float t = 0;
        while (t < trailLifetime)
        {
            trail.time = Mathf.Lerp(trail.time, Mathf.Lerp(trailTime, 0, t / trailLifetime), 0.5f);
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
        }

        trail.emitting = false;
    }

    private void EnableAnimations()
    {
        canSwitchAnimation = true;
    }

    public Directions GetCurrentDir()
    {
        return currentDir;
    }
}