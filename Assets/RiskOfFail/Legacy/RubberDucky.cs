using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubberDucky : InteractBase
{
    public float moveSpeed;
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public Vector3 offset;

    public bool canExit = true;
    public bool riding;
    protected override void PlayerInteract()
    {
        riding = !riding;
    }

    private void FixedUpdate()
    {
        if (riding)
        {
            Vector2 playerDirection = new Vector2(KeyBind.GetAxis(KInputManager.GetKey("Right"),
                KInputManager.GetKey("Left")), KeyBind.GetAxis(KInputManager.GetKey("Up"), KInputManager.GetKey("Down"))).normalized;

            rb.AddForce(playerDirection * moveSpeed * Time.fixedDeltaTime);

            playerTransform.position = transform.position + new Vector3(offset.x * (sprite.flipX ? -1 : 1), offset.y);

            sprite.flipX = !(rb.linearVelocity.x < 0);
        }
    }
}
