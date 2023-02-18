using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFade : MonoBehaviour
{
    [SerializeField] private float fullFadeOpacity = 0.25f;
    float targetOpacity = 1;

    SpriteRenderer sprite;
    Transform player;
    [SerializeField] float yPos;

    private void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        player = PlayerStatus.player.transform;

        yPos = sprite.bounds.min.y + .5f;
    }
    
    private void FixedUpdate()
    {
        targetOpacity = player.position.y > yPos ? fullFadeOpacity : 1;

        Color c = sprite.color;
        c.a = Mathf.Lerp(c.a, targetOpacity, .25f);

        //c.a = Mathf.Lerp(0, targetOpacity, Mathf.Abs(vertDistanceToPlayer) / fullFadeDistance);


        sprite.color = Color.Lerp(sprite.color, c, 0.5f);

    }
}
