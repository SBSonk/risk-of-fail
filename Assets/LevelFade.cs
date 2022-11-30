using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelFade : MonoBehaviour
{
    Image img;
    private void Awake()
    {
        img = GetComponent<Image>();
    }

    void Start()
    {
        // Fade in
        StartCoroutine(SprFunctions.Fade(img, Color.black, Color.clear, 0.5f));   
    }
}
                