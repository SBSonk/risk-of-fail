using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFade : MonoBehaviour
{
    [SerializeField] Transform anchor;
    [SerializeField] private int fullSort = 3, normalSort = 0;
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
        if (!anchor) return;
        
        if (player.position.y > anchor.position.y)
        {
            targetOpacity = fullFadeOpacity;
            sprite.sortingOrder = fullSort;
        }
        else
        {
            targetOpacity = 1;
            sprite.sortingOrder = normalSort;
        }

        Color c = sprite.color;
        c.a = Mathf.Lerp(c.a, targetOpacity, .25f);

        //c.a = Mathf.Lerp(0, targetOpacity, Mathf.Abs(vertDistanceToPlayer) / fullFadeDistance);


        sprite.color = Color.Lerp(sprite.color, c, 0.5f);

    }
}
