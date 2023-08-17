using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class FollowBehindPlayer : MonoBehaviour
{
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private Vector3 directionOffset, playerOffset;
    [SerializeField] private float offsetMultiplier = 2f, lerpVal = 0.25f, rotateVal = 0.1f;

    private bool collected;
    public UnityEvent ItemPickedUp;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player") || collected) return;

        collected = true;
        ItemPickedUp?.Invoke();
        player = col.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!player) return;
        
        if (player.velocity.magnitude > 0.5f)
        {
            switch (VectorToDir(player.velocity))
            {
                case Directions.up:
                    directionOffset = Vector3.Slerp(directionOffset, new Vector3(0, -1), rotateVal);
                    break;

                case Directions.right:
                    directionOffset = Vector3.Slerp(directionOffset, new Vector3(-1, 0), rotateVal);
                    break;

                case Directions.down:
                    directionOffset = Vector3.Slerp(directionOffset, new Vector3(0, 1), rotateVal);
                    break;

                case Directions.left:
                    directionOffset = Vector3.Slerp(directionOffset, new Vector3(1, 0), rotateVal);
                    break;
                
                case Directions.bottomLeft:
                    directionOffset = Vector3.Slerp(directionOffset, new Vector3(1, 1), rotateVal);
                    break;
                
                case Directions.bottomRight:
                    directionOffset = Vector3.Slerp(directionOffset, new Vector3(-1, 1), rotateVal);
                    break;
                
                case Directions.upperLeft:
                    directionOffset = Vector3.Slerp(directionOffset, new Vector3(1, -1), rotateVal);
                    break;
                
                case Directions.upperRight:
                    directionOffset = Vector3.Slerp(directionOffset, new Vector3(-1, -1), rotateVal);
                    break;
            }
        }

        transform.position = Vector3.Slerp(transform.position, (Vector3) player.position + playerOffset + (directionOffset * offsetMultiplier), lerpVal);
    }
    
    Directions VectorToDir(Vector2 input)
    {
        Directions final = Directions.right;
        float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg; // angle in degrees
        int direction = Mathf.RoundToInt(angle / 45.0f) % 8; // direction as integer from 0 to 7 // this is cap, chatgpt lied, it uses negative ints for up and down

        switch(direction)
        {
            case 0:
                final = Directions.right;
                break;

            case 1:
                final = Directions.upperRight;
                break;

            case -1:
                final = Directions.bottomRight;
                break;

            case 2:
                final = Directions.up;
                break;

            case -2:
                final = Directions.down;
                break;

            case 3:
                final = Directions.upperLeft;
                break;

            case -3:
                final = Directions.bottomLeft;
                break;

            case 4:
                final = Directions.left;
                break;
        }    

        return final;
    }   
}
