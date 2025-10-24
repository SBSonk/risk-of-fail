using UnityEngine;

public class PaintSplatter : MonoBehaviour
{
    public Sprite[] possibleSprites;
    private SpriteRenderer spr;

    // Start is called before the first frame update
    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        spr = GetComponent<SpriteRenderer>();

        // Randomize sprite
        if (possibleSprites.Length > 0)
            spr.sprite = possibleSprites[Random.Range(0, possibleSprites.Length)];

        // Randomize rotation
        transform.Rotate(0, 0, Random.Range(0, 360));

        // Randomize size
        transform.localScale = Vector3.one * Random.Range(0.5f, 1.51f);

        // Random displacement
        transform.position += Vector3.one * Random.Range(0, .5f);

        Invoke("FadeOut", 5f);
    }

    private void FadeOut()
    {
        StartCoroutine(HelperFunctions.Fade(spr, spr.color, Color.clear, 3f));
        Destroy(gameObject, 3f);
    }
}