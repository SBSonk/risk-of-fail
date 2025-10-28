using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAfterSeconds : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Color fadeColor;

    [SerializeField] private float timeToFade = 8f;
    [SerializeField] private float fadeTime = 2f;

    private void Start()
    {
        Invoke("StartFade", timeToFade);
    }

    void StartFade() => StartCoroutine(HelperFunctions.Fade(sprite, sprite.color, fadeColor, fadeTime));
}
