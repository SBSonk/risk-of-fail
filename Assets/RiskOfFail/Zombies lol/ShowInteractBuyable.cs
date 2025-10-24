using TMPro;
using UnityEngine;

public class ShowInteractBuyable : ShowInteract
{
    public TMP_Text priceText;
    private bool active = true;
    private Buyable buyable;

    private void Start()
    {
        buyable = GetComponent<Buyable>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!active) return;

        base.OnTriggerEnter2D(collision);
        if (!collision.CompareTag("Player")) return;
        priceText.text = buyable.price.ToString();
        StartCoroutine(HelperFunctions.Fade(priceText, priceText.color, Color.white, fadeTime));
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (!collision.CompareTag("Player")) return;
        StartCoroutine(HelperFunctions.Fade(priceText, priceText.color, Color.clear, fadeTime));
    }

    public void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(HelperFunctions.Fade(priceText, priceText.color, Color.clear, fadeTime));
        StartCoroutine(HelperFunctions.Fade(interactIcon, interactIcon.color, Color.clear, fadeTime));

        active = false;
    }
}