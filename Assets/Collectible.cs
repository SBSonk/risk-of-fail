using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : PickupBase
{
    public Color transparentColor = new Color(1, 1, 1, .25f);

    protected override void Start()
    {
        base.Start();
        
        LoadCollectStatus();

        OnPickupCollect.AddListener(SaveCollectStatus);
    }

    void LoadCollectStatus()
    {
        bool collectedBefore = PlayerPrefs.GetInt(transform.parent.name + "Collectable", 0) == 1 ? true : false;

        if (collectedBefore) sprite.GetComponent<SpriteRenderer>().color = transparentColor;
    }

    void SaveCollectStatus()
    {
        PlayerPrefs.SetInt(transform.parent.name + "Collectable", 1);
    }
}
