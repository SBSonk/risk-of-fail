using System;
using System.Collections;
using RiskOfFail.Combat;
using RiskOfFail.Combat.Effects;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Stunnable))]
public class PlayerMovement : MonoBehaviour
{
    public enum PlayerStates
    {
        CanMove,
        Dashing,
        Stuck
    }

    public PlayerStates state = PlayerStates.CanMove;

    [Header("Movement")]
    public float moveSpeed = 10f;
    public float stunSpeed = 2;
    public float baseSpeed;
    
    [Header("Dodging")]
    public float dodgeForce = 100f;
    public float dodgeCooldown = 0.25f, dodgeResetStartTime = 0.1f, dodgeInvincibilityTime = .5f;
    public int maxDodges = 2;
    
    float dodges;

    // EVENTS
    public UnityEvent OnDodge;
    public UnityEvent OnWalkStart, OnWalkEnd;

    private Stunnable stunManager;
    
    private bool dodgeQueued, startCooldown, canDodge = true;
    private Vector2 moveDirection;

    private Rigidbody2D rb;
    private bool walking;

    private void Awake()
    {
        // Initialize player
        rb = GetComponent<Rigidbody2D>();
        stunManager = GetComponent<Stunnable>();
    }

    private void OnEnable()
    {
        Initialize();
    }

    // movement input
    private void Update()
    {
        if (PauseMenu.paused) return;

        GetInputs();

        if (moveDirection.magnitude > 0 && !walking)
        {
            OnWalkStart?.Invoke();
            ;
            walking = true;
        }
        else if (moveDirection.magnitude == 0 && walking)
        {
            OnWalkEnd?.Invoke();
            walking = false;
        }

        // Replenishes dodges
        if (dodges < maxDodges && startCooldown) dodges += Time.deltaTime / dodgeCooldown;
        canDodge = dodges >= 1;

        // Queue dodge if player can dodge and is moving.
        if (dodgeQueued) Dodge(moveDirection);
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    void Initialize()
    {
        dodges = maxDodges;
    }
    
    void GetInputs()
    {
        moveDirection = new Vector2(KeyBind.GetAxis(KInputManager.GetKey("Right"),
                    KInputManager.GetKey("Left")),
                KeyBind.GetAxis(KInputManager.GetKey("Up"), KInputManager.GetKey("Down")))
            .normalized;
        
        if (KInputManager.GetKey("Dash").PressedDown() && moveDirection.magnitude > 0) dodgeQueued = true;
    }
    
    public void SetSpeedMultiplier(float multiplier)
    {
        moveSpeed = baseSpeed * multiplier;
    }

    public void ResetSpeed()
    {
        moveSpeed = baseSpeed;
    }

    private void HandleMovement()
    {
        float speed = stunManager.isStunned ? stunSpeed : moveSpeed;
        
        rb.linearVelocity = moveDirection * speed;
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
        //PlayerStatus.player.immune = val;
    }

    private IEnumerator DodgeIFrames()
    {
        SetIFrame(true);

        yield return new WaitForSeconds(dodgeInvincibilityTime);

        SetIFrame(false);
    }

    private void StartDodgeCooldown()
    {
        startCooldown = true;
    }
}