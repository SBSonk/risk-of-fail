using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using RiskOfFail.Combat;
using RiskOfFail.Combat.Effects;

[RequireComponent(typeof(HasStun))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public enum PlayerState
    {
        CanMove,
        Dashing,
        Stuck
    }

    [Header("State")]
    public PlayerState state = PlayerState.CanMove;

    [Header("Movement Settings")]
    public float baseSpeed = 10f;
    public float moveSpeed = 10f;
    public float stunSpeedMultiplier = .25f;
    [Range(0f, 0.5f)] public float movementSmoothing = 0.05f;

    [Header("Dodge Settings")]
    public float dodgeForce = 100f;
    public float dodgeDuration = 0.25f;
    public float dodgeCooldown = 0.25f;
    public float dodgeResetStartTime = 0.1f;
    public float dodgeInvincibilityTime = 0.5f;
    public float dodgeBufferTime = 0.15f;   // ⬅ NEW: time window to buffer dash input
    public int maxDodges = 2;

    [Header("Events")]
    public UnityEvent OnDodge;
    public UnityEvent OnWalkStart;
    public UnityEvent OnWalkEnd;

    // Internal
    private Rigidbody2D rb;
    private HasStun stunManager;
    private Vector2 moveDirection;
    private Vector2 smoothVelocity;
    private bool walking;
    private bool startCooldown;
    private bool canDodge = true;
    private float dodges;

    // Input buffer
    private float dodgeBufferTimer = 0f;
    private bool bufferActive = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stunManager = GetComponent<HasStun>();
        baseSpeed = moveSpeed;
    }

    private void OnEnable() => Initialize();

    private void Initialize()
    {
        dodges = maxDodges;
        startCooldown = false;
        canDodge = true;
        dodgeBufferTimer = 0f;
        bufferActive = false;
    }

    private void Update()
    {
        if (PauseMenu.paused) return;

        GetInputs();
        HandleWalkEvents();
        HandleDodgeReplenish();
        HandleDodgeBuffer();

        // Attempt dodge immediately if conditions allow
        if (canDodge && moveDirection.sqrMagnitude > 0 && (bufferActive || dodgeBufferTimer > 0))
            TryDodge(moveDirection);
    }

    private void FixedUpdate()
    {
        if (state == PlayerState.CanMove)
            HandleMovement();
    }

    // --- INPUT HANDLING ---
    private void GetInputs()
    {
        moveDirection = new Vector2(
            KeyBind.GetAxis(KInputManager.GetKey("Right"), KInputManager.GetKey("Left")),
            KeyBind.GetAxis(KInputManager.GetKey("Up"), KInputManager.GetKey("Down"))
        ).normalized;

        // Register dodge input with buffer
        if (KInputManager.GetKey("Dash").PressedDown())
        {
            if (canDodge && moveDirection.sqrMagnitude > 0)
            {
                // Immediate dodge if available
                TryDodge(moveDirection);
            }
            else
            {
                // Start buffer window
                bufferActive = true;
                dodgeBufferTimer = dodgeBufferTime;
            }
        }
    }

    // --- MOVEMENT ---
    private void HandleMovement()
    {
        float currentSpeed = stunManager.isStunned ? moveSpeed * stunSpeedMultiplier : moveSpeed;
        Vector2 targetVelocity = moveDirection * currentSpeed;

        // Apply smoothing only when not dashing
        if (state == PlayerState.CanMove)
        {
            rb.linearVelocity = Vector2.SmoothDamp(
                rb.linearVelocity,
                targetVelocity,
                ref smoothVelocity,
                movementSmoothing
            );
        }
        else
        {
            // Move instantly when dashing or stuck
            rb.linearVelocity = targetVelocity;
            smoothVelocity = Vector2.zero;
        }
    }


    private void HandleWalkEvents()
    {
        bool isMoving = moveDirection.sqrMagnitude > 0.01f;

        if (isMoving && !walking)
        {
            OnWalkStart?.Invoke();
            walking = true;
        }
        else if (!isMoving && walking)
        {
            OnWalkEnd?.Invoke();
            walking = false;
        }
    }

    // --- DODGE SYSTEM ---
    private void HandleDodgeBuffer()
    {
        if (bufferActive)
        {
            dodgeBufferTimer -= Time.deltaTime;
            if (dodgeBufferTimer <= 0f)
                bufferActive = false;
        }
    }

    private void TryDodge(Vector2 direction)
    {
        if (!canDodge || direction.sqrMagnitude <= 0)
            return;

        // Clear buffer since we're now dodging
        bufferActive = false;
        dodgeBufferTimer = 0f;

        dodges--;
        canDodge = false;
        StartCoroutine(DodgeRoutine(direction));
    }

    private IEnumerator DodgeRoutine(Vector2 direction)
    {
        SetState(PlayerState.Dashing);
        OnDodge?.Invoke();

        // Apply instant impulse
        rb.AddForce(direction * dodgeForce, ForceMode2D.Impulse);

        // Start separate timers
        StartCoroutine(DodgeIFrames());
        StartCoroutine(DodgeEndAfterDuration());

        // Cooldown setup
        startCooldown = false;
        CancelInvoke(nameof(StartDodgeCooldown));
        Invoke(nameof(StartDodgeCooldown), dodgeResetStartTime);

        yield return null;
    }

    private IEnumerator DodgeIFrames()
    {
        SetIFrame(true);
        yield return new WaitForSeconds(dodgeInvincibilityTime);
        SetIFrame(false);
    }

    private IEnumerator DodgeEndAfterDuration()
    {
        yield return new WaitForSeconds(dodgeDuration);
        SetState(PlayerState.CanMove);
    }

    private void StartDodgeCooldown() => startCooldown = true;

    private void HandleDodgeReplenish()
    {
        if (dodges < maxDodges && startCooldown)
            dodges += Time.deltaTime / dodgeCooldown;

        canDodge = dodges >= 1;
    }

    // --- UTILITIES ---
    public void SetSpeedMultiplier(float multiplier) => moveSpeed = baseSpeed * multiplier;
    public void ResetSpeed() => moveSpeed = baseSpeed;
    private void SetState(PlayerState s) => state = s;
    private void SetIFrame(bool val)
    {
        // Hook to your immunity system here if needed
        // PlayerStatus.player.immune = val;
    }
}
