using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Tiny class I made for simple sprite animations
public class SprFunctions : MonoBehaviour
{
    ///=====================FADE=========================\\\

    // Fades a sprite between two colors within a time period
    public static IEnumerator Fade(SpriteRenderer sprite, Color startColor, Color finalColor, float t)
    {
        sprite.color = startColor;
        Color color = startColor;
        float time = 0;
        while (time <= t)
        {
            sprite.color = color;
            color = Color.Lerp(startColor, finalColor, time / t);

            yield return new WaitForEndOfFrame();
            time += Time.unscaledDeltaTime;
        }

        sprite.color = finalColor;
    }

    public static IEnumerator Fade(Image sprite, Color startColor, Color finalColor, float t)
    {
        sprite.color = startColor;
        Color color = startColor;
        float time = 0;
        while (time <= t)
        {
            sprite.color = color;
            color = Color.Lerp(startColor, finalColor, time / t);

            yield return new WaitForEndOfFrame();
            time += Time.unscaledDeltaTime;
        }

        sprite.color = finalColor;
    }

    ///=====================FLICKER=========================\\\

    // Flickers a sprite and picks up speed the closer to the end of the timeframe
    public static IEnumerator Flicker(SpriteRenderer sprite, float t)
    {
        float flickerInt = .15f;
        float fastFlickerInt = flickerInt / 1.5f;
        float reallyFastFlickerInt = flickerInt / 2;
        float timeRemaining = t;
        float time20 = t * .4f;
        float time10 = t * .2f;

        // Flicker animation
        bool active = true;
        Color current = sprite.color;
        while (timeRemaining > 0)
        {
            active = !active;
            current.a = active ? 1 : 0;
            sprite.color = current;

            yield return new WaitForSeconds(flickerInt);
            timeRemaining -= flickerInt;

            if (timeRemaining <= time10) flickerInt = reallyFastFlickerInt;
            else if (timeRemaining <= time20) flickerInt = fastFlickerInt;

        }

        current.a = 1;
        sprite.color = current;
    }

    public static IEnumerator Flicker(Image sprite, float t)
    {
        float flickerInt = .15f;
        float fastFlickerInt = flickerInt / 1.5f;
        float reallyFastFlickerInt = flickerInt / 2;
        float timeRemaining = t;
        float time20 = t * .4f;
        float time10 = t * .2f;

        // Flicker animation
        bool active = true;
        Color current = sprite.color;
        while (timeRemaining > 0)
        {
            active = !active;
            current.a = active ? 1 : 0;
            sprite.color = current;

            yield return new WaitForSeconds(flickerInt);
            timeRemaining -= flickerInt;

            if (timeRemaining <= time10) flickerInt = reallyFastFlickerInt;
            else if (timeRemaining <= time20) flickerInt = fastFlickerInt;

        }

        current.a = 1;
        sprite.color = current;
    }
}