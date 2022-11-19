using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;

    bool dodgeQueued, startCooldown, canDodge = true;
    public float dodgeForce = 100f;
    public float dodgeCooldown = 0.25f, dodgeResetStartTime = 0.1f;
    public int maxDodges = 2;
    public float dodges;

    Rigidbody2D rb;
    Vector2 moveDirection;

    // EVENTS
    public delegate void OnDodge();
    public static event OnDodge onDodge;

    private void Start()
    {
        // Initialize player
        rb = GetComponent<Rigidbody2D>();

        dodges = maxDodges;
    }

    // movement input
    void Update()
    {
        moveDirection = InputManager.playerDirection;

        // Replenishes dodges
        if (dodges < maxDodges && startCooldown) dodges += Time.deltaTime / dodgeCooldown;

        canDodge = dodges >= 1;

        // Queue dodge if player can dodge and is moving.
        if (canDodge && InputManager.dodge && moveDirection.magnitude > 0) dodgeQueued = true;
    }

    // player movement with movespeed value
    void FixedUpdate()
    {
        moveCharacter(moveDirection);
    }

    void moveCharacter(Vector2 direction)
    {
        rb.AddForce(direction * moveSpeed * Time.fixedDeltaTime);

        // Scuffed dodge implementation
        if (dodgeQueued)
        {
            rb.AddForce(direction * dodgeForce, ForceMode2D.Impulse);
            dodgeQueued = false;

            // Dodge cooldown
            CancelInvoke("StartDodgeCooldown");
            startCooldown = false;
            Invoke("StartDodgeCooldown", dodgeResetStartTime);

            dodges--;
            onDodge.Invoke();
        }
    }

    void StartDodgeCooldown()
    {
        startCooldown = true;
    }
}
