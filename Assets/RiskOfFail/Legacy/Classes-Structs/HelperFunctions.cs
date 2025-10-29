using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

// Tiny class I made for simple sprite animations
public class HelperFunctions
{
    public static Directions VectorToDir(Vector2 input, bool allowDiagonals = false)
    {
        var final = Directions.right;

        if (allowDiagonals)
        {
            // Threshold for direction detection
            var threshold = 0.5f;

            var right = input.x > threshold;
            var left = input.x < -threshold;
            var up = input.y > threshold;
            var down = input.y < -threshold;

            if (up && right) final = Directions.upperRight;
            else if (up && left) final = Directions.upperLeft;
            else if (down && right) final = Directions.bottomRight;
            else if (down && left) final = Directions.bottomLeft;
            else if (right) final = Directions.right;
            else if (left) final = Directions.left;
            else if (up) final = Directions.up;
            else if (down) final = Directions.down;
        }
        else
        {
            // Original 4-direction logic
            if (input.x > 0.5f) final = Directions.right;
            else if (input.x < -0.5f) final = Directions.left;
            else if (input.y > 0.5f) final = Directions.up;
            else if (input.y < -0.5f) final = Directions.down;
        }

        return final;
    }

    public static Vector2 DirToVector(Directions dir)
    {
        switch (dir)
        {
            case Directions.right:
                return Vector2.right;
            case Directions.left:
                return Vector2.left;
            case Directions.up:
                return Vector2.up;
            case Directions.down:
                return Vector2.down;
            case Directions.upperRight:
                return new Vector2(1, 1).normalized;
            case Directions.upperLeft:
                return new Vector2(-1, 1).normalized;
            case Directions.bottomRight:
                return new Vector2(1, -1).normalized;
            case Directions.bottomLeft:
                return new Vector2(-1, -1).normalized;
            default:
                return Vector2.zero;
        }
    }

    public static float VectorToAngle(Vector2 input)
    {
        var angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;

        return angle;
    }

    #region FADE

    ///=====================FADE=========================\\\

    // Fades a sprite between two colors within a time period
    public static IEnumerator Fade(SpriteRenderer sprite, Color startColor, Color finalColor, float t,
        Action onFinished = null)
    {
        sprite.color = startColor;
        var color = startColor;
        float time = 0;
        while (time <= t)
        {
            sprite.color = color;
            color = Color.Lerp(startColor, finalColor, time / t);

            yield return new WaitForEndOfFrame();
            time += Time.unscaledDeltaTime;
        }

        sprite.color = finalColor;

        onFinished?.Invoke();
    }

    public static IEnumerator Fade(SpriteRenderer[] sprites, Color startColor, Color finalColor, float t,
        Action onFinished = null)
    {
        for (var i = 0; i < sprites.Length; i++) sprites[i].color = startColor;

        var color = startColor;
        float time = 0;
        while (time <= t)
        {
            for (var i = 0; i < sprites.Length; i++)
            {
                sprites[i].color = color;
                color = Color.Lerp(startColor, finalColor, time / t);
            }

            yield return new WaitForEndOfFrame();
            time += Time.unscaledDeltaTime;
        }

        for (var i = 0; i < sprites.Length; i++) sprites[i].color = finalColor;

        onFinished?.Invoke();
    }

    public static IEnumerator Fade(Image sprite, Color startColor, Color finalColor, float t, Action onFinished = null)
    {
        sprite.color = startColor;
        var color = startColor;
        float time = 0;
        while (time <= t)
        {
            sprite.color = color;
            color = Color.Lerp(startColor, finalColor, time / t);

            yield return new WaitForEndOfFrame();
            time += Time.unscaledDeltaTime;
        }

        sprite.color = finalColor;

        onFinished?.Invoke();
    }

    public static IEnumerator Fade(TMP_Text text, Color startColor, Color finalColor, float t, Action onFinished = null)
    {
        text.color = startColor;
        var color = startColor;
        float time = 0;
        while (time <= t)
        {
            text.color = color;
            color = Color.Lerp(startColor, finalColor, time / t);

            yield return new WaitForEndOfFrame();
            time += Time.unscaledDeltaTime;
        }

        text.color = finalColor;

        onFinished?.Invoke();
    }

    #endregion

    #region FLICKER

    ///=====================FLICKER=========================\\\

    // Flickers a sprite and picks up speed the closer to the end of the timeframe
    public static IEnumerator Flicker(SpriteRenderer sprite, float t, Action onFinished = null)
    {
        var flickerInt = .15f;
        var fastFlickerInt = flickerInt / 1.5f;
        var reallyFastFlickerInt = flickerInt / 2;
        var timeRemaining = t;
        var time20 = t * .4f;
        var time10 = t * .2f;

        // Flicker animation
        var active = true;
        var current = sprite.color;
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

        onFinished?.Invoke();
    }

    public static IEnumerator Flicker(Image sprite, float t, Action onFinished = null)
    {
        var flickerInt = .15f;
        var fastFlickerInt = flickerInt / 1.5f;
        var reallyFastFlickerInt = flickerInt / 2;
        var timeRemaining = t;
        var time20 = t * .4f;
        var time10 = t * .2f;

        // Flicker animation
        var active = true;
        var current = sprite.color;
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

        onFinished?.Invoke();
    }

    #endregion
}

[Serializable]
public enum Directions
{
    up = 0,
    right = 1,
    down = 2,
    left = 3,
    bottomRight = 4,
    bottomLeft = 5,
    upperRight = 6,
    upperLeft = 7
}