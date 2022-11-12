using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public float openDelay = 0.5f;
    public float closeDelay = 0f;

    Collider2D col;
    SpriteRenderer sprite;
    Color startCol;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        startCol = sprite.color;
        startCol.a = 1;

        InitializeDoor();
    }

    void InitializeDoor()
    {
        sprite.color = col.enabled ? startCol : Color.clear;
    }

    public void ToggleDoor(bool val)
    {
        float delay = val ? closeDelay : openDelay;
        if (closeDelay < 0.1f) delay = 0.1f;

        Color endCol = val ? startCol : Color.clear;

        StartCoroutine(SprFunctions.Fade(sprite, sprite.color, endCol, delay + 0.1f));
        StartCoroutine(Toggle(val, delay));
    }

    IEnumerator Toggle(bool val, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        col.enabled = val;
    }
}
