using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFade : MonoBehaviour
{
    public float fullFadeDistance = 0.5f;
    float targetOpacity = 1;

    SpriteRenderer sprite;
    Transform player;
    float yPos;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        player = PlayerStatus.player.transform;

        // Offset fade start to top of collider
        yPos = GetComponent<Collider2D>().bounds.min.y;
    }

    private void FixedUpdate()
    {
        float vertDistanceToPlayer = player.position.y - yPos;
        targetOpacity = vertDistanceToPlayer > fullFadeDistance ? 0 : 1;

        Color c = sprite.color;
        if (vertDistanceToPlayer > fullFadeDistance)
        {
            c.a = Mathf.Lerp(0, targetOpacity, Mathf.Abs(vertDistanceToPlayer) / fullFadeDistance);
        }
        else
        {
            c.a = 1;
        }

        //c.a = Mathf.Lerp(0, targetOpacity, Mathf.Abs(vertDistanceToPlayer) / fullFadeDistance);


        sprite.color = Color.Lerp(sprite.color, c, 0.5f);
        sprite.sortingOrder = vertDistanceToPlayer > fullFadeDistance ? 100 : 0;

        print(vertDistanceToPlayer);
    }
}
