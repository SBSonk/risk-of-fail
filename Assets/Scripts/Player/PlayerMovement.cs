using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float baseSpeed;

    bool dodgeQueued, startCooldown, canDodge = true;
    public float dodgeForce = 100f;
    public float dodgeCooldown = 0.25f, dodgeResetStartTime = 0.1f;
    public int maxDodges = 2;
    public float dodges;

    Rigidbody2D rb;
    Vector2 moveDirection;
    private bool walking;
    
    // EVENTS
    public UnityEvent OnDodge;
    public UnityEvent OnWalkStart, OnWalkEnd;

    private void Start()
    {
        // Initialize player
        rb = GetComponent<Rigidbody2D>();

        dodges = maxDodges;
    }

    // movement input
    void Update()
    {
        if (Time.timeScale == 0) return;
        
        Vector2 playerDirection = new Vector2(KeyBind.GetAxis(KInputManager.GetKey("Right"),
            KInputManager.GetKey("Left")), KeyBind.GetAxis(KInputManager.GetKey("Up"), KInputManager.GetKey("Down"))).normalized;
        moveDirection = playerDirection;

        if (playerDirection.magnitude > 0 && !walking)
        {
            OnWalkStart?.Invoke();;
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

    void FixedUpdate()
    {
        MoveCharacter(moveDirection);
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        moveSpeed = baseSpeed * multiplier;
    }

    public void ResetSpeed() => moveSpeed = baseSpeed;

    void MoveCharacter(Vector2 dir)
    {
        rb.AddForce(dir * moveSpeed * Time.fixedDeltaTime);

        if (dir.magnitude == 0 && !PlayerStatus.player.stunned) rb.linearDamping = 20;
        else rb.linearDamping = 11;
    }
 
    void Dodge(Vector2 direction)
    {
        rb.AddForce(direction * dodgeForce, ForceMode2D.Impulse);
        dodgeQueued = false;

        // Dodge cooldown
        CancelInvoke("StartDodgeCooldown");
        startCooldown = false;
        Invoke("StartDodgeCooldown", dodgeResetStartTime);

        dodges--;
        OnDodge.Invoke();
    }

    void StartDodgeCooldown()
    {
        startCooldown = true;
    }
}
