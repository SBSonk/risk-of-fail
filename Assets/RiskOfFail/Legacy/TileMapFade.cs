using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapFade : MonoBehaviour
{
    [SerializeField] private float fullFadeOpacity = 0.25f;
    [SerializeField] private int fullSort = 2, normalSort;

    [SerializeField] private ObjectFade[] objectsToFade;
    [SerializeField] private float yPos;

    private Tilemap sprite;
    private TilemapRenderer spriteRenderer;
    private float targetOpacity = 1;

    private void Start()
    {
        sprite = GetComponent<Tilemap>();
        spriteRenderer = GetComponent<TilemapRenderer>();

        yPos = transform.TransformPoint(0, yPos, 0).y;
    }

    private void FixedUpdate()
    {
        if (!ObjectFade.player)
        {
            Debug.LogWarning("Object Fade Failed");
            return;
        }

        if (ObjectFade.player.position.y > yPos)
        {
            targetOpacity = fullFadeOpacity;
            spriteRenderer.sortingOrder = fullSort;

            if (objectsToFade.Length > 0)
                foreach (var objectFade in objectsToFade)
                    objectFade.UnFade();
        }
        else
        {
            targetOpacity = 1;
            spriteRenderer.sortingOrder = normalSort;

            if (objectsToFade.Length > 0)
                foreach (var objectFade in objectsToFade)
                    objectFade.Fade();
        }

        var c = sprite.color;
        c.a = Mathf.Lerp(c.a, targetOpacity, .25f);

        //c.a = Mathf.Lerp(0, targetOpacity, Mathf.Abs(vertDistanceToPlayer) / fullFadeDistance);


        sprite.color = Color.Lerp(sprite.color, c, 0.5f);
    }
}