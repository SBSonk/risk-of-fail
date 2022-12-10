using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInteract : MonoBehaviour
{
    public float fadeTime = 0.1f;
    public SpriteRenderer outline, interactIcon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        StopAllCoroutines();
        StartCoroutine(SprFunctions.Fade(outline, outline.color, Color.white, fadeTime));
        StartCoroutine(SprFunctions.Fade(interactIcon, interactIcon.color, Color.white, fadeTime));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        StopAllCoroutines();
        StartCoroutine(SprFunctions.Fade(outline, outline.color, Color.clear, fadeTime));
        StartCoroutine(SprFunctions.Fade(interactIcon, interactIcon.color, Color.clear, fadeTime));
    }
}
