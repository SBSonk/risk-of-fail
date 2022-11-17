using System.Collections;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public bool canSwitchAnimation = true   ;
    [SerializeField] SpriteRenderer[] sprites;

    [SerializeField] ParticleSystem dashExp;
    [SerializeField] Animator animator;
    [SerializeField] Animator weaponAnimator;

    [Header("Dodge")]
    [SerializeField] TrailRenderer trail;
    [SerializeField] float trailLifetime = 0.5f;
    [SerializeField] ScreenshakeValue dodgeScreenshake;
    [SerializeField] GameObject playerGhost;
    [SerializeField] int copies = 4;
    [SerializeField] float timeBetweenCopies, ghostLifetime = 0.25f;

    [Header("Player Sprite")]
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] float stopCursorFollowTime = 3f;
    Directions currentDir = Directions.down;
    Vector2 input, dir;
    Transform cursor;
    MoveCursor cursorScript;
    bool followCursor;

    [Header("Weapon")]
    [SerializeField] Weapon heldWeapon;
    string animType = "A";
    [SerializeField] SpriteRenderer weaponSprite;
    [SerializeField] Transform weapon;
    [SerializeField] float weaponLerp = 0.5f;

    PlayerShooting shooting;

    private void Awake()
    {
        cursor = GameObject.Find("PlayerCursor").transform; 
        cursorScript = cursor.GetComponent<MoveCursor>();
        shooting = GetComponent<PlayerShooting>();
    }

    private void Start()
    {
        var shooting = PlayerStatus.player.pShooting;

        shooting.OnShoot.AddListener(ShootAnimation);
        shooting.OnWeaponSwitch.AddListener(ChangeWeaponSprite);
        shooting.OnShove.AddListener(ShoveAnimation);
        shooting.OnMelee.AddListener(MeleeAnimation);

        PlayerMovement.onDodge += DodgeAnimation;
        GameManager.onWeaponReceive += ChangeWeaponSprite;
        PlayerStatus.onPlayerDamage += DamageAnimation;

        ChangeWeaponSprite();
    }

    private void OnDestroy()
    {
        PlayerMovement.onDodge -= DodgeAnimation;
        GameManager.onWeaponReceive -= ChangeWeaponSprite;
        PlayerStatus.onPlayerDamage -= DamageAnimation;
    }

    private void Update()
    {
        input = InputManager.playerDirection;

        if (InputManager.shootAuto || InputManager.shoot || InputManager.shove)
        {
            followCursor = true;
            CancelInvoke();
        }
        else if (followCursor) Invoke("StopCursorFollow", stopCursorFollowTime);

        // Orient player
        dir = ((Vector3) InputManager.mousePosition - transform.position).normalized;

        currentDir = VectorToDir(dir);

        // Orient weapon
        Vector2 mousePos = ((Vector2)transform.position - InputManager.mousePosition).normalized;
        float angle = Mathf.Atan2(-mousePos.y, -mousePos.x) * Mathf.Rad2Deg;

        weapon.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.LerpAngle(weapon.rotation.eulerAngles.z, angle, weaponLerp)));
    } 

    private void FixedUpdate()
    {
        // Choose animations
        if (!canSwitchAnimation) return;

        // Flip if going left
        sprite.flipX = currentDir == Directions.left;

        if (input.magnitude > 0)
        {
            switch (currentDir)
            {
                case Directions.up:
                    animator.Play("walk_u");
                    break;

                case Directions.right:
                    animator.Play("walk_r");
                    break;

                case Directions.down:
                    animator.Play("walk_d");
                    break;

                case Directions.left:
                    animator.Play("walk_r");
                    break;
            }
        }
        else
        {
            switch (currentDir)
            {
                case Directions.up:
                    animator.Play("stand_u");
                    break;

                case Directions.right:
                    animator.Play("stand_r");
                    break;

                case Directions.down:
                    animator.Play("stand_d");
                    break;

                case Directions.left:
                    animator.Play("stand_r");
                    break;
            }
        }
    }

    void MeleeAnimation()
    {
        weaponAnimator.CrossFade("Swing", .25f, 0, 0f);
    }

    void ShootAnimation()
    {
        weaponAnimator.CrossFade("Shoot" + animType, .25f, 0, 0f);
    }
    void ShoveAnimation()
    {
        weaponAnimator.CrossFade("Shove" + animType, .25f);
    }

    void ChangeWeaponSprite()
    {        
        var weapon = shooting.GetHeldWeapon();

        weaponSprite.sprite = weapon.weapon.weaponSprite;

        heldWeapon = weapon.weapon;

        switch(heldWeapon.animType)
        {
            case AnimationTypes.Light:
                animType = "A";
                break;

            case AnimationTypes.Brush:
                animType = "B";
                break;

            case AnimationTypes.Heavy:
                animType = "C";
                break;

            default:
                animType = "A";
                break;
        }

        weaponAnimator.CrossFade("Hold" + animType, .25f);

        cursorScript.ChangeCrosshair(weapon.weapon.crossHair);
    }

    void DodgeAnimation()
    {
        dashExp.Play();
        StartCoroutine(DashTrail());
        StartCoroutine(DashGhosts());
        // TODO: make it so that the trail only appears if speed is above a threshold

        // Face the direction when dodging
        StopCursorFollow();
        weaponAnimator.CrossFade("Hold" + animType, .25f);

        CameraFunctions.main.DoScreenShake(dodgeScreenshake);
    }


    void DamageAnimation()
    {
        canSwitchAnimation = false;
        
        switch (currentDir)
        {
            case Directions.up:
                animator.Play("hurt_u");
                break;

            case Directions.right:
                animator.Play("hurt_r");
                break;

            case Directions.down:
                animator.Play("hurt_d");
                break;

            case Directions.left:
                animator.Play("hurt_r");
                break;
        }
        Invoke("EnableAnimations", 0.5f);
    }

    IEnumerator DashTrail()
    {
        // Enable trail
        trail.emitting = true;

        yield return new WaitForSeconds(trailLifetime);

        // Retract trail
        trail.emitting = false;
    }

    IEnumerator DashGhosts()
    {
        int i = 0;

        float t = 0;
        float tInc = timeBetweenCopies / copies;
        while (i < copies)
        {
            var g = Instantiate(playerGhost, transform.position, Quaternion.identity, null).GetComponent<PlayerGhost>();
            for (int o = 0; o < sprites.Length; o++)
            {
                g.sprites[o].sprite = sprites[o].sprite;
                g.sprites[o].flipX = sprites[o].flipX;
            }

            StartCoroutine(g.GhostAnimation(ghostLifetime - (timeBetweenCopies * i - 1)));
            i++;
            t += tInc;
            yield return new WaitForSeconds(t);
        }
    }

    Directions VectorToDir(Vector2 input)
    {
        Directions final = currentDir;

        if (input.x > 0.5f) final = Directions.right;
        else if (input.x < -0.5f) final = Directions.left;
        else if (input.y > 0.5f) final = Directions.up;
        else if (input.y < -0.5f) final = Directions.down;

        return final;
    }   

    void StopCursorFollow()
    {
        followCursor = false;
    }

    void EnableAnimations() { canSwitchAnimation = true; }
}

[System.Serializable]
public enum Directions
{
    up, right, down, left
}