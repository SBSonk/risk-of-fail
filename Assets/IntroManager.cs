using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public GameObject introVideo;
    public GameObject uiCanvas;

    public bool seenOpening;

    private void Awake()
    {
        CheckFirstLaunch();
    }

    private void Start()
    {
        Show();
    }

    void CheckFirstLaunch()
    {
        seenOpening = PlayerPrefs.GetInt("LaunchedBefore", 0) == 1 ? true : false;
    }

    void Show()
    {
        if (seenOpening)
        {
            uiCanvas.SetActive(true);
        }
        else
        {
            introVideo.SetActive(true);
            seenOpening = true;

            PlayerPrefs.SetInt("LaunchedBefore", 1);
        }
    }

    [ContextMenu("ResetCheck")]
    void ResetCheck() => PlayerPrefs.SetInt("LaunchedBefore", 0);
}
