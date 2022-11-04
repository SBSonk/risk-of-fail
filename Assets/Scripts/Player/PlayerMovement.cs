using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;

    bool dodgeQueued, canDodge = true;
    public float dodgeForce = 100f;
    public float dodgeCooldown = 0.25f;
    public int maxDodges = 2;
    public int dodges;

    Rigidbody2D rb;
    Vector2 moveDirection;

    // EVENTS
    public delegate void OnDodge();
    public static event OnDodge onDodge;

    public UnityEvent OnDodgeRecharge;

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
        canDodge = dodges > 0;

        // Queue dodge if player can dodge and is moving.
        if (canDodge && InputManager.dodge && moveDirection.magnitude > 0 && dodges > 0) dodgeQueued = true;
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
            removeDodge(dodgeCooldown);
            onDodge.Invoke();
        }
    }

    public void removeDodge(float length)
    {
        if (dodges > 0)
            dodges--;

        // Increase dodge cooldown if player used up all dodges
        length = dodges == 0 ? length * 1.5f : length;

        CancelInvoke();
        Invoke("resetDodge", length);
    }

    void resetDodge() 
    {
        if (dodges < maxDodges)
        {
            dodges++;
            Invoke("resetDodge", dodgeCooldown);

            OnDodgeRecharge?.Invoke();
        }
    } // Only exists so i can make an invoke call lol
}
