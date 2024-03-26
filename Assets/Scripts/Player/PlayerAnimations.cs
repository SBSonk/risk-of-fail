using FirstGearGames.SmoothCameraShaker;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{ 
    Vector2 playerOffset = new Vector2(0, 0.5f);
    
    public bool canSwitchAnimation = true   ;
    [SerializeField] SpriteRenderer[] sprites;

    [SerializeField] ParticleSystem dashExp;
    [SerializeField] Animator animator;

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
    bool followCursor = true;

    [Header("Weapon")] [SerializeField] private WeaponAnimator weaponAnimator;
    [SerializeField] float weaponLerp = 0.5f;
    public ShakeData shoveShake;

    [SerializeField] ResultsScreen results;
    private Rigidbody2D rb;

    private float angle = 0;

    public void Initialize(PlayerShooting shooting, PlayerMovement movement, PlayerStatus status)
    {
        cursor = GameObject.Find("PlayerCursor").transform;
        cursorScript = cursor.GetComponent<MoveCursor>();
 
        shooting.OnShoot.AddListener(ShootAnimation);
        shooting.OnWeaponSwitch.AddListener(ChangeWeaponSprite);
        shooting.OnShove.AddListener(ShoveAnimation);
        shooting.OnMelee.AddListener(ShootAnimation);

        movement.OnDodge.AddListener(DodgeAnimation);

        status.onHit.AddListener(DamageAnimation);
        status.onDeath.AddListener(DeathAnimation);

        ChangeWeaponSprite(shooting.GetHeldWeapon());

        trailTime = trail.time;
        trail.time = 0;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Time.timeScale == 0) return;
        
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 playerDirection = new Vector2(KeyBind.GetAxis(KInputManager.GetKey("Right"),
            KInputManager.GetKey("Left")), KeyBind.GetAxis(KInputManager.GetKey("Up"), KInputManager.GetKey("Down")));
        input = playerDirection;

        if (KInputManager.GetKey("Shoot").Pressed() || KInputManager.GetKey("Shoot").PressedDown() || KInputManager.GetKey("Shove").PressedDown())
        {
            followCursor = true;
            canSwitchAnimation = true;
            CancelInvoke();
        }
        else if (followCursor) Invoke("StopCursorFollow", stopCursorFollowTime);

        // Orient player
        dir = ((Vector3) mousePosition - (transform.position + (Vector3)playerOffset)).normalized;

        currentDir = VectorToDir(dir);

        // Change weapon sorting order depending on if its in front or behind
        if (weaponAnimator.sprite && canSwitchAnimation)
        {
            if (currentDir == Directions.down) weaponAnimator.sprite.sortingOrder = 1;
            else weaponAnimator.sprite.sortingOrder = 0;
            
            // Orient weapon
            if (followCursor)
            {
                Vector2 mousePos = ((Vector2)transform.position + playerOffset - mousePosition).normalized;
                angle = Mathf.Atan2(-mousePos.y, -mousePos.x) * Mathf.Rad2Deg;
            }
        }
        
        weaponAnimator.transform.rotation = Quaternion.Euler(new Vector3(0, 0,
            Mathf.LerpAngle(weaponAnimator.transform.rotation.eulerAngles.z, angle, weaponLerp)));
    }

    private void FixedUpdate()
    {
        // Choose animations
        if (!canSwitchAnimation) return;

        // Flip if going left
        sprite.flipX = currentDir == Directions.left || currentDir == Directions.upperLeft || currentDir == Directions.bottomLeft;

        if (input.magnitude > 0 && rb.velocity.magnitude > 0)
        {
            Vector2 playerDirection = new Vector2(KeyBind.GetAxis(KInputManager.GetKey("Right"),
                KInputManager.GetKey("Left")), KeyBind.GetAxis(KInputManager.GetKey("Up"), KInputManager.GetKey("Down")));
            var inputDir = VectorToDir(playerDirection);
            
            switch (currentDir)
            {
                case Directions.up:
                    animator.Play("walk_u");
                    break;

                /*case Directions.upperRight:
                    animator.Play("walk_ur");
                    break;

                case Directions.upperLeft:
                    animator.Play("walk_ur");
                    break;*/

                case Directions.right:
                    animator.Play("walk_r");
                    break;

                /*case Directions.bottomRight:
                    animator.Play("walk_dr");
                    break;

                case Directions.bottomLeft:
                    animator.Play("walk_dr");
                    break;*/

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

                case Directions.upperRight:
                    animator.Play("stand_ur");
                    break;

                case Directions.upperLeft:
                    animator.Play("stand_ur");
                    break;

                case Directions.right:
                    animator.Play("stand_r");
                    break;

                case Directions.bottomRight:
                    animator.Play("stand_dr");
                    break;

                case Directions.bottomLeft:
                    animator.Play("stand_dr");
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

    [ContextMenu("kYS")]
    void DeathAnimation(KillFlag deathType) => StartCoroutine(DeathAnim(deathType));

    IEnumerator DeathAnim(KillFlag deathType)
    {
        canSwitchAnimation = false;
        sprite.sortingOrder = 100;
        animator.Play("player_death");
        
        GetComponent<Collider2D>().enabled = false;
        
        rb.drag = 0.01f;
        rb.gravityScale = 3;
        rb.AddForce(new Vector3(Mathf.Sign(rb.velocity.x) * 10, 20), ForceMode2D.Impulse);

        if (weaponAnimator)
        {
            weaponAnimator.gameObject.SetActive(false);

            var weaponRb = Instantiate(PlayerStatus.player.pShooting.GetHeldWeapon().weapon.rigidbodyVariant,
                transform.position, transform.rotation);
            weaponRb.AddForce(new Vector3(-Mathf.Sign(rb.velocity.x) * 10, 20), ForceMode2D.Impulse);
            weaponRb.AddTorque(-Mathf.Sign(rb.velocity.x) * 10f, ForceMode2D.Impulse);
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

        results.ShowResults(deathType);
    }
    
    void ShootAnimation()
    {
        weaponAnimator.PlayShootAnimation();
    }
    void ShoveAnimation()
    {
        weaponAnimator.PlayShoveAnimation();
        
        StartCoroutine(ShoveShake(.1f));
    }

    IEnumerator ShoveShake(float t)
    {
        yield return new WaitForSeconds(t);
        if (shoveShake) CameraShakerHandler.Shake(shoveShake);
    }

    public void ChangeWeaponSprite(InventoryWeapon w)
    {
        cursorScript.ChangeCrosshair(w.weapon.hud.crossHair);

        // Replace weapon object
        Destroy(weaponAnimator.gameObject);
        weaponAnimator = Instantiate(w.weapon.animatorController, transform.position + new Vector3(0, 1.25f) + w.weapon.weaponOffset, Quaternion.identity, sprite.transform);
        weaponAnimator.transform.localScale = w.weapon.weaponScale;
        
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos = (mousePosition - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(-mousePos.y, -mousePos.x) * Mathf.Rad2Deg;

        // Change weapon sorting order depending on if its in front or behind
        if (weaponAnimator.sprite)
        {
            if (currentDir == Directions.down) weaponAnimator.sprite.sortingOrder = 1;
            else weaponAnimator.sprite.sortingOrder = 0;
        }
        
        weaponAnimator.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
    }
    

    void DodgeAnimation()
    {
        Directions dir = VectorToDir(input);
        switch (dir)
        {
            case Directions.up:
                dashExp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                animator.Play("dash_u");
                break;

            case Directions.right:
                dashExp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                animator.Play("dash_r");
                break;

            case Directions.down:
                dashExp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                animator.Play("dash_d");
                break;

            case Directions.left:
                dashExp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
                animator.Play("dash_r");
                break;
        }
        
        sprite.flipX = dir == Directions.left || currentDir == Directions.upperLeft || currentDir == Directions.bottomLeft;


        dashExp.Play();
        /*StopCoroutine("DashTrail");
        StartCoroutine(DashTrail());*/

        StopCursorFollow();

        CameraShakerHandler.Shake(dodgeScreenshake);

        followCursor = false;
        
        Vector2 dirInput = (input).normalized;
        angle = Mathf.Atan2(dirInput.y, dirInput.x) * Mathf.Rad2Deg;
        
        if (dir == Directions.down) weaponAnimator.sprite.sortingOrder = 1;
        else weaponAnimator.sprite.sortingOrder = 0;
        
        canSwitchAnimation = false;
        CancelInvoke();
        Invoke("EnableAnimations", 0.3f);
    }

    void DamageAnimation(float _)
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

    void EnableAnimations()
    {
        followCursor = true; canSwitchAnimation = true;
    }
}

[System.Serializable]
public enum Directions
{
    up, right, down, left, bottomRight, bottomLeft, upperRight, upperLeft
}