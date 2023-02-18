using UnityEngine;
using System.Collections;
using Pathfinding;

public class Door : MonoBehaviour
{
    public float openDelay = 0.5f;
    public float closeDelay = 0f;
    public Sprite openedSprite, closedSprite;
    private SpriteRenderer sprite;

    public bool opened;

    Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        ToggleDoor(opened);
    }

    public void ToggleDoor(bool val)
    {
        float delay = val ? closeDelay : openDelay;
        if (closeDelay < 0.1f) delay = 0.1f;

        StartCoroutine(Toggle(val, delay));
    }
    
    [ContextMenu("ToggleDoor")]
    public void ToggleDoor()
    {
        bool val = !opened;
        
        float delay = val ? closeDelay : openDelay;
        if (closeDelay < 0.1f) delay = 0.1f;

        StartCoroutine(Toggle(val, delay));
    }

    IEnumerator Toggle(bool val, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        opened = val;

        sprite.sprite = opened ? openedSprite : closedSprite;
        col.enabled = !opened;

        AstarPath.active.UpdateGraphs(col.bounds, 1);
    }
}
