using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkSlots : MonoBehaviour
{
    public static PerkSlots main;
    
    public Vector3 anchorPos = new Vector3(-40, 25);
    public float xSpacing = 50;

    public RectTransform template;
    public int perksOwned = 0;

    private void Awake()
    {
        main = this;
    }

    public void AddIcon(Sprite image)
    {
        var newIcon = Instantiate(template, transform);
        newIcon.localPosition = anchorPos + new Vector3(xSpacing * perksOwned, 0);
        newIcon.GetComponent<Image>().sprite = image;
        perksOwned++;
    }

}
