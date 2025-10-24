using System.Collections;
using UnityEngine;

public class PlayerGhost : MonoBehaviour
{
    public SpriteRenderer[] sprites;
    public float timeToDestroy = 0.5f;

    public IEnumerator GhostAnimation(float time)
    {
        float t = 0;

        var spriteStartColors = new Color[sprites.Length];

        for (var i = 0; i < sprites.Length; i++) spriteStartColors[i] = sprites[i].color;

        // Start fading
        while (t < time)
        {
            for (var i = 0; i < sprites.Length; i++)
                sprites[i].color = Color.Lerp(spriteStartColors[i], Color.clear, t / timeToDestroy);

            yield return new WaitForSeconds(0.05f);

            t += 0.05f;
        }

        Destroy(gameObject);
    }
}