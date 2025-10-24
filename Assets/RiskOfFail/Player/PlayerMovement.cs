using System.Collections;
using RiskOfFail.Combat;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerStates
    {
        CanMove,
        Dashing,
        Stuck
    }

    public PlayerStates state = PlayerStates.CanMove;

    public float moveSpeed = 10f;
    public float baseSpeed;
    public float dodgeForce = 100f;
    public float dodgeCooldown = 0.25f, dodgeResetStartTime = 0.1f, dodgeIFrames = .5f;
    public int maxDodges = 2;
    public float dodges;

    // EVENTS
    public UnityEvent OnDodge;
    public UnityEvent OnWalkStart, OnWalkEnd;

    private bool dodgeQueued, startCooldown, canDodge = true;
    private Vector2 moveDirection;

    private Rigidbody2D rb;
    private bool walking;

    private void Start()
    {
        // Initialize player
        rb = GetComponent<Rigidbody2D>();

        dodges = maxDodges;
    }

    // movement input
    private void Update()
    {
        if (Time.timeScale == 0) return;

        var playerDirection = new Vector2(KeyBind.GetAxis(KInputManager.GetKey("Right"),
                KInputManager.GetKey("Left")),
            KeyBind.GetAxis(KInputManager.GetKey("Up"), KInputManager.GetKey("Down")))
            .normalized;
        moveDirection = playerDirection;

        if (playerDirection.magnitude > 0 && !walking)
        {
            OnWalkStart?.Invoke();
            ;
            walking = true;
        }
        else if (playerDirection.magnitude == 0 && walking)
        {
            OnWalkEnd?.Invoke();
            walking = false;
        }

        // Replenishes dodges
        if (dodges < maxDodges && startCooldown) dodges += Time.deltaTime / dodgeCooldown;
        canDodge = dodges >= 1;

        // Queue dodge if player can dodge and is moving.
        if (canDodge && KInputManager.GetKey("Dash").PressedDown() && moveDirection.magnitude > 0) dodgeQueued = true;
        if (dodgeQueued) Dodge(moveDirection);
    }

    private void FixedUpdate()
    {
        MoveCharacter(moveDirection);
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        moveSpeed = baseSpeed * multiplier;
    }

    public void ResetSpeed()
    {
        moveSpeed = baseSpeed;
    }

    private void MoveCharacter(Vector2 dir)
    {
        rb.AddForce(dir * moveSpeed * Time.fixedDeltaTime);

        if (dir.magnitude == 0 && !PlayerStatus.player.stunned) rb.linearDamping = 20;
        else rb.linearDamping = 11;
    }

    private void Dodge(Vector2 direction)
    {
        StartCoroutine(DodgeIFrames());

        rb.AddForce(direction * dodgeForce, ForceMode2D.Impulse);
        dodgeQueued = false;

        // Dodge cooldown
        CancelInvoke("StartDodgeCooldown");
        startCooldown = false;
        Invoke("StartDodgeCooldown", dodgeResetStartTime);

        dodges--;
        OnDodge.Invoke();
    }

    private void SetIFrame(bool val)
    {
        PlayerStatus.player.immune = val;
    }

    private IEnumerator DodgeIFrames()
    {
        SetIFrame(true);

        yield return new WaitForSeconds(dodgeIFrames);

        SetIFrame(false);
    }

    private void StartDodgeCooldown()
    {
        startCooldown = true;
    }
}