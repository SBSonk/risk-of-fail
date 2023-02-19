using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFade : MonoBehaviour
{
    [SerializeField] Transform anchor;
    [SerializeField] private float fullFadeOpacity = 0.25f;
    float targetOpacity = 1;

    SpriteRenderer sprite;
    Transform player;

    private void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        player = PlayerStatus.player.transform;
    }
    
    private void FixedUpdate()
    {
        if (player.position.y > anchor.position.y)
        {
            targetOpacity = fullFadeOpacity;
            sprite.sortingOrder = 3;
        }
        else
        {
            targetOpacity = 1;
            sprite.sortingOrder = 0;
        }

        Color c = sprite.color;
        c.a = Mathf.Lerp(c.a, targetOpacity, .25f);

        //c.a = Mathf.Lerp(0, targetOpacity, Mathf.Abs(vertDistanceToPlayer) / fullFadeDistance);


        sprite.color = Color.Lerp(sprite.color, c, 0.5f);

    }
}
