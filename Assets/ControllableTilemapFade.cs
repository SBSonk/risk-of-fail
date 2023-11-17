using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ControllableTilemapFade : MonoBehaviour
{
    [SerializeField] private bool fadeOnStart = false;
    [SerializeField] private float fullFadeOpacity = 0.25f;
    [SerializeField] private int fullSort = 1, normalSort = 0;

    [SerializeField] private ObjectFade[] objectsToFade;
    float targetOpacity = 1;

    Tilemap sprite;
    TilemapRenderer spriteRenderer;
    

    private void Start()
    {
        sprite = GetComponent<Tilemap>();
        spriteRenderer = GetComponent<TilemapRenderer>();
        
        if (fadeOnStart) FadeTiles();
    }

    private void FixedUpdate()
    {
        if (!ObjectFade.player) return;

        Color c = sprite.color;
        c.a = Mathf.Lerp(c.a, targetOpacity, .25f);

        //c.a = Mathf.Lerp(0, targetOpacity, Mathf.Abs(vertDistanceToPlayer) / fullFadeDistance);

        sprite.color = Color.Lerp(sprite.color, c, 0.5f);
    }

    public void UnfadeTiles()
    {
        targetOpacity = 1;
        spriteRenderer.sortingOrder = normalSort;

        if (objectsToFade.Length > 0)
        {
            foreach (ObjectFade objectFade in objectsToFade)
            {
                objectFade.Fade();
            }
        }
    }

    public void FadeTiles()
    {
        targetOpacity = fullFadeOpacity;
        spriteRenderer.sortingOrder = fullSort;
        
        if (objectsToFade.Length > 0)
        {
            foreach (ObjectFade objectFade in objectsToFade)
            {
                objectFade.UnFade();
            }
        }
    }
}
