using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapFade : MonoBehaviour
{
    [SerializeField] private float fullFadeOpacity = 0.25f;
    [SerializeField] private int fullSort = 2, normalSort = 0;
    float targetOpacity = 1;

    Tilemap sprite;
    TilemapRenderer spriteRenderer;
    Transform player;
    [SerializeField] float yPos;

    private void Start()
    {
        sprite = GetComponent<Tilemap>();
        spriteRenderer = GetComponent<TilemapRenderer>();
        player = PlayerStatus.player.transform;

        yPos = transform.TransformPoint(0, yPos, 0).y;
    }
    
    private void FixedUpdate()
    {
        if (player.position.y > yPos)
        {
            targetOpacity = fullFadeOpacity;
            spriteRenderer.sortingOrder = fullSort;
        }
        else
        {
            targetOpacity = 1;
            spriteRenderer.sortingOrder = normalSort;
        }

        Color c = sprite.color;
        c.a = Mathf.Lerp(c.a, targetOpacity, .25f);

        //c.a = Mathf.Lerp(0, targetOpacity, Mathf.Abs(vertDistanceToPlayer) / fullFadeDistance);


        sprite.color = Color.Lerp(sprite.color, c, 0.5f);
    }
}
