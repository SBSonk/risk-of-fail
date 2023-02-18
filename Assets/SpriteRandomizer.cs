using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpriteRandomizer : MonoBehaviour
{
    public Sprite[] sprites;
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (sprites.Length == 0)
        {
            print("No sprites to randomize...");
            return;
        }
        
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }
}
