using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollectAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer keySprite;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        keySprite.sortingOrder = 0;
    }
}
