using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTileFader : MonoBehaviour
{
    public ControllableTilemapFade[] tilesToFade;

    public void Fade()
    {
        foreach (ControllableTilemapFade tile in tilesToFade)
        {
            tile.FadeTiles();
        }
    }
    
    public void UnFade()
    {
        foreach (ControllableTilemapFade tile in tilesToFade)
        {
            tile.UnfadeTiles();
        }
    }
}
