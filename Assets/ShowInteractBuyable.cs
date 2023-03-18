using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowInteractBuyable : ShowInteract
{
    public TMP_Text priceText;
    private Buyable buyable;

    private void Start()
    {
        buyable = GetComponent<Buyable>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        priceText.text = buyable.price.ToString();
        StartCoroutine(SprFunctions.Fade(priceText, priceText.color, Color.white, fadeTime));
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        
        StartCoroutine(SprFunctions.Fade(priceText, priceText.color, Color.clear, fadeTime));
    }
}
