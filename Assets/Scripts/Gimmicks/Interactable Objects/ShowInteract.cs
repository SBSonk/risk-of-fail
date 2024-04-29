using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInteract : MonoBehaviour
{
    public bool active = true;
    
    public float fadeTime = 0.1f;
    public SpriteRenderer outline, interactIcon;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        
        outline.sprite = interactIcon.sprite;
        StopAllCoroutines();
        StartCoroutine(SprFunctions.Fade(outline, outline.color, Color.white, fadeTime));
        StartCoroutine(SprFunctions.Fade(interactIcon, interactIcon.color, Color.white, fadeTime));
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        StopAllCoroutines();
        StartCoroutine(SprFunctions.Fade(outline, outline.color, Color.clear, fadeTime));
        StartCoroutine(SprFunctions.Fade(interactIcon, interactIcon.color, Color.clear, fadeTime));
    }
}
