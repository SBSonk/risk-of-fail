using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;

    bool dodgeQueued, canDodge = true;
    public float dodgeForce = 100f;
    public float dodgeCooldown = 0.25f;

    Rigidbody2D rb;
    Vector2 moveDirection;

    // EVENTS
    public delegate void OnDodge();
    public static event OnDodge onDodge;

    private void Start()
    {
        // Initialize player
        rb = GetComponent<Rigidbody2D>();
    }

    // movement input
    void Update()
    {
        moveDirection = InputManager.playerDirection;

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
            onDodge.Invoke();
            
            dodgeQueued = false;

            // Dodge cooldown
            removeDodge(dodgeCooldown);
        }
    }

    public void removeDodge(float length)
    {
        canDodge = false;

        Invoke("resetDodge", length);
    }

    void resetDodge() { canDodge = true; } // Only exists so i can make an invoke call lol
}
