using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

// Tiny class I made for simple sprite animations
public class HelperFunctions
{
    #region FADE

    ///=====================FADE=========================\\\

    // Fades a sprite between two colors within a time period
    public static IEnumerator Fade(SpriteRenderer sprite, Color startColor, Color finalColor, float t, Action onFinished = null)
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
        
        onFinished?.Invoke();
    }

    public static IEnumerator Fade(SpriteRenderer[] sprites, Color startColor, Color finalColor, float t, Action onFinished = null)
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].color = startColor;
        }

        Color color = startColor;
        float time = 0;
        while (time <= t)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].color = color;
                color = Color.Lerp(startColor, finalColor, time / t);
            }

            yield return new WaitForEndOfFrame();
            time += Time.unscaledDeltaTime;
        }

        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].color = finalColor;
        }
        
        onFinished?.Invoke();
    }

    public static IEnumerator Fade(Image sprite, Color startColor, Color finalColor, float t, Action onFinished = null)
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
        
        onFinished?.Invoke();
    }
    
    public static IEnumerator Fade(TMP_Text text, Color startColor, Color finalColor, float t, Action onFinished = null)
    {
        text.color = startColor;
        Color color = startColor;
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
        
        onFinished?.Invoke();
    }

    public static IEnumerator Flicker(Image sprite, float t, Action onFinished = null)
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
        
        onFinished?.Invoke();
    }

    #endregion
    
    public static Directions VectorToDir(Vector2 input, bool allowDiagonals = false)
    {
        Directions final = Directions.right;

        if (allowDiagonals)
        {
            // Threshold for direction detection
            float threshold = 0.5f;

            bool right = input.x > threshold;
            bool left = input.x < -threshold;
            bool up = input.y > threshold;
            bool down = input.y < -threshold;

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
        float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;

        return angle;
    }
}

[System.Serializable]
public enum Directions
{
    up, right, down, left, bottomRight, bottomLeft, upperRight, upperLeft
}

