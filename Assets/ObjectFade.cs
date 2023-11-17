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
    public static Transform player;

    private void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    
    private void FixedUpdate()
    {
        Color c = sprite.color;
        c.a = Mathf.Lerp(c.a, targetOpacity, .25f);

        //c.a = Mathf.Lerp(0, targetOpacity, Mathf.Abs(vertDistanceToPlayer) / fullFadeDistance);
        sprite.color = Color.Lerp(sprite.color, c, 0.5f);
        
        if (!anchor || !player) return;
        
        if (player.position.y > anchor.position.y)
        {
            UnFade();
        }
        else
        {
            Fade();
        }

        

    }

    public void Fade()
    {
        targetOpacity = 1;
        sprite.sortingOrder = normalSort;
    }

    public void UnFade()
    {
        targetOpacity = fullFadeOpacity;
        sprite.sortingOrder = fullSort;
    }
}
