using System.Collections;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public bool canSwitchAnimation = true   ;

    [SerializeField] ParticleSystem dashExp;
    [SerializeField] Animator animator;

    [Header("Dodge")]
    [SerializeField] TrailRenderer trail;
    [SerializeField] float trailLifetime = 0.5f;
    [SerializeField] ScreenshakeValue dodgeScreenshake;

    [Header("Player Sprite")]
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] float stopCursorFollowTime = 3f;
    Directions currentDir = Directions.down;
    Vector2 input, dir;
    Transform cursor;
    bool followCursor;

    private void Awake()
    {
        cursor = GameObject.Find("PlayerCursor").transform;
    }

    private void Start()
    {
        PlayerMovement.onDodge += DodgeAnimation;
        PlayerStatus.onPlayerDamage += DamageAnimation;
    }

    private void OnDestroy()
    {
        PlayerMovement.onDodge -= DodgeAnimation;
        PlayerStatus.onPlayerDamage -= DamageAnimation;
    }

    private void Update()
    {
        input = InputManager.playerDirection;

        if (InputManager.shootAuto || InputManager.shoot)
        {
            followCursor = true;
            CancelInvoke();
        }
        else if (followCursor) Invoke("StopCursorFollow", stopCursorFollowTime);

        // Get directions (prioritize vertical directions)
        if (followCursor)
        {
            // Get direction from me to mouse
            dir = (cursor.position - transform.position).normalized;

            currentDir = VectorToDir(dir);
        }
        else currentDir = VectorToDir(input);
    } 

    private void FixedUpdate()
    {
        // Flip if going left
        sprite.flipX = currentDir == Directions.left;

        // Choose animations
        if (!canSwitchAnimation) return;

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

    void DodgeAnimation()
    {
        dashExp.Play();
        StartCoroutine(DashTrail());
        // TODO: make it so that the trail only appears if speed is above a threshold

        // Face the direction when dodging
        StopCursorFollow();

        CameraFunctions.main.DoScreenShake(dodgeScreenshake);
    }

    void DamageAnimation()
    {
        animator.Play("PlayerHit");
    }

    IEnumerator DashTrail()
    {
        // Enable trail
        trail.emitting = true;

        yield return new WaitForSeconds(trailLifetime);

        // Retract trail
        trail.emitting = false;
    }

    Directions VectorToDir(Vector2 input)
    {
        Directions final = currentDir;

        if (input.y > 0.5f) final = Directions.up;
        else if (input.y < -0.5f) final = Directions.down;
        else if (input.x > 0.5f) final = Directions.right;
        else if (input.x < -0.5f) final = Directions.left;

        return final;
    }

    void StopCursorFollow()
    {
        followCursor = false;
    }

    enum Directions
    {
        up, right, down, left
    }
}
