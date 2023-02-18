using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapFade : MonoBehaviour
{
    [SerializeField] private float fullFadeOpacity = 0.25f;
    float targetOpacity = 1;

    Tilemap sprite;
    Transform player;
    [SerializeField] float yPos;

    private void Start()
    {
        sprite = GetComponent<Tilemap>();
        player = PlayerStatus.player.transform;

        yPos = transform.TransformPoint(0, yPos, 0).y;
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
