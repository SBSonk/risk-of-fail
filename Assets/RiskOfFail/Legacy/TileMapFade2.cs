using System.Collections;
using System.Collections.Generic;
using RiskOfFail.Combat;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapFade2 : MonoBehaviour
{
    [SerializeField] private float fullFadeOpacity = 0.25f;
    [SerializeField] private int fullSort = 0, normalSort = 0;
    float targetOpacity = 1;

    Tilemap sprite;
    TilemapRenderer spriteRenderer;
    [SerializeField] Transform anchor;

    public Transform playerTransform;

    private void Start()
    {
        sprite = GetComponent<Tilemap>();
        spriteRenderer = GetComponent<TilemapRenderer>();

        playerTransform = PlayerStatus.instance.transform;
    }
    
    private void FixedUpdate()
    {
        if (!ObjectFade.player) return;

        
        Vector3 anchorFadePos = anchor.position;
        anchorFadePos.x = playerTransform.position.x;
        float distanceToPlayerFromEdge = Vector3.Distance(anchorFadePos, playerTransform.position);
        Debug.DrawLine(anchorFadePos, playerTransform.position, Color.red);
        if (ObjectFade.player.position.y > anchor.position.y && distanceToPlayerFromEdge <= 10)
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
