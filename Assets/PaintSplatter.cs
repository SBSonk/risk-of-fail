using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintSplatter : MonoBehaviour
{
    SpriteRenderer spr;

    // Start is called before the first frame update
    public void Initialize(Sprite[] possibleSprites)
    {
        spr = GetComponent<SpriteRenderer>();

        // Randomize sprite
        if (possibleSprites.Length > 0)
            spr.sprite = possibleSprites[Random.Range(0, possibleSprites.Length)];

        // Randomize rotation
        transform.Rotate(0, 0, Random.Range(0, 360));

        // Randomize size
        transform.localScale = Vector3.one * Random.Range(0.75f, 1.26f);

        Invoke("FadeOut", 5f);
    }

    void FadeOut()
    {
        StartCoroutine(SprFunctions.Fade(spr, spr.color, Color.clear, 3f));
        Destroy(gameObject, 3f);
    }
}
