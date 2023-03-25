using FirstGearGames.SmoothCameraShaker;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimations : NetworkBehaviour        
{
    public bool canSwitchAnimation = true   ;
    [SerializeField] SpriteRenderer[] sprites;

    [SerializeField] ParticleSystem dashExp;
    [SerializeField] Animator animator;
    [SerializeField] Animator weaponAnimator;

    [Header("Dodge")]
    [SerializeField] TrailRenderer trail;
    [SerializeField] float trailLifetime = 0.5f;
    [SerializeField] ShakeData dodgeScreenshake;
    float trailTime;

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
    public ShakeData shoveShake;

    [SerializeField] ResultsScreen results;
    private Rigidbody2D rb;

    private PlayerStatus player;

    public void Initialize(PlayerShooting shooting, PlayerMovement movement, PlayerStatus status)
    {
        player = status;
        
        cursor = GameObject.Find("PlayerCursor").transform;
        cursorScript = cursor.GetComponent<MoveCursor>();
 
        shooting.OnShoot.AddListener(ShootAnimation);
        shooting.OnWeaponSwitch.AddListener(UpdateWeaponSpriteClientRpc);
        shooting.OnShove.AddListener(ShoveAnimation);
        shooting.OnMelee.AddListener(MeleeAnimation);

        movement.OnDodge.AddListener(DodgeAnimationClientRpc);

        status.OnPlayerDamage.AddListener(DamageAnimation);
        status.onDeath.AddListener(DeathAnimation);

        UpdateWeaponSpriteClientRpc();

        trailTime = trail.time;
        trail.time = 0;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!IsOwner) return;
        
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
        OrientWeapon();
    } 

    private void FixedUpdate()
    {
        // Choose animations
        if (!canSwitchAnimation || !IsOwner) return;
        
        FaceDirection();
    }
    
    void OrientWeapon()
    {
        Vector2 mousePos = ((Vector2)transform.position - InputManager.mousePosition).normalized;
        float angle = Mathf.Atan2(-mousePos.y, -mousePos.x) * Mathf.Rad2Deg;
        
        UpdateWeaponSortingClientRpc();

        weapon.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.LerpAngle(weapon.rotation.eulerAngles.z, angle, weaponLerp)));
    }

    [ClientRpc]
    void UpdateWeaponSortingClientRpc()
    {
        // Change weapon sorting order depending on if its in front or behind
        if (currentDir == Directions.down || currentDir == Directions.bottomRight || currentDir == Directions.bottomLeft) weaponSprite.sortingOrder = 1;
        else weaponSprite.sortingOrder = 0;
    }
    
    
    void FaceDirection()
    {
        if (input.magnitude > 0 && rb.velocity.magnitude > 0)
        {
            var inputDir = VectorToDir(InputManager.playerDirection);
            
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
                    animator.Play("walk_l");
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
                    animator.Play("stand_l");
                    break;
            }
        }}
    
    void DeathAnimation() => StartCoroutine(DeathAnim());

    IEnumerator DeathAnim()
    {
        canSwitchAnimation = false;
        sprite.sortingOrder = 100;
        weaponSprite.enabled = false;
        animator.Play("player_death");

        GetComponent<Collider2D>().enabled = false;

        var rb = GetComponent<Rigidbody2D>();
        rb.drag = 0.01f;
        rb.gravityScale = 3;
        rb.AddForce(new Vector3(Mathf.Sign(rb.velocity.x) * 10, 20), ForceMode2D.Impulse);


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

        results.ShowResults();
    }
    
    void MeleeAnimation()
    {
        weaponAnimator.CrossFade("Swing", .1f, 0, 0f);
    }
    
    void ShootAnimation()
    {
        weaponAnimator.CrossFade("Shoot" + animType, .1f, 0, 0f);
    }
    
    void ShoveAnimation()
    {
        weaponAnimator.CrossFade("Shove" + animType, .1f);

        if (IsOwner)
        {
            StartCoroutine(ShoveShake(.1f));
        }
    }
    
    IEnumerator ShoveShake(float t)
    {
        yield return new WaitForSeconds(t);
        if (shoveShake) CameraShakerHandler.Shake(shoveShake);
    }

    [ClientRpc]
    public void UpdateWeaponSpriteClientRpc()
    {
        var w = player.pShooting.GetHeldWeapon();
        
        weaponSprite.sprite = w.weapon.weaponSprite;
        weaponSprite.transform.localPosition = w.weapon.weaponOffset;
        weaponSprite.transform.localScale = w.weapon.weaponScale;

        heldWeapon = w.weapon;

        switch(heldWeapon.effects.animType)
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

            case AnimationTypes.Melee:
                animType = "Melee";
                break;

            default:
                animType = "A";
                break;
        }

        weaponAnimator.CrossFade("Hold" + animType, .25f);

        cursorScript.ChangeCrosshair(w.weapon.hud.crossHair);
    }

    [ClientRpc]
    void DodgeAnimationClientRpc()
    {
        switch (VectorToDir(input))
        {
            case Directions.up:
                dashExp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;

            case Directions.right:
                dashExp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                break;

            case Directions.down:
                dashExp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                break;

            case Directions.left:
                dashExp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
                break;
        }


        dashExp.Play();
        StopCoroutine("DashTrail");
        StartCoroutine(DashTrail());

        StopCursorFollow();

        CameraShakerHandler.Shake(dodgeScreenshake);
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
                animator.Play("hurt_l");
                break;
        }
        Invoke("EnableAnimations", 0.5f);
    }

    IEnumerator DashTrail()
    {
        // Enable trail
        trail.time = trailTime;
        
        float t = 0;
        while (t < trailLifetime)
        {
            trail.time = Mathf.Lerp(trail.time, Mathf.Lerp(trailTime, 0, t / trailLifetime), 0.25f);
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
        }
    }

    Directions VectorToDir(Vector2 input)
    {
        Directions final = currentDir;
        if (input.x > 0.5f) final = Directions.right;
        else if (input.x < -0.5f) final = Directions.left;
        else if (input.y > 0.5f) final = Directions.up;
        else if (input.y < -0.5f) final = Directions.down;
        
        /*float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg; // angle in degrees
        int direction = Mathf.RoundToInt(angle / 45.0f) % 8; // direction as integer from 0 to 7 // this is cap, chatgpt lied, it uses negative ints for up and down

        switch(direction)
        {
            case 0:
                final = Directions.right;
                break;

            case 1:
                final = Directions.upperRight;
                break;

            case -1:
                final = Directions.bottomRight;
                break;

            case 2:
                final = Directions.up;
                break;

            case -2:
                final = Directions.down;
                break;

            case 3:
                final = Directions.upperLeft;
                break;

            case -3:
                final = Directions.bottomLeft;
                break;

            case 4:
                final = Directions.left;
                break;
        }    */

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
    up, right, down, left, bottomRight, bottomLeft, upperRight, upperLeft
}