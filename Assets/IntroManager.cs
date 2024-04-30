using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class IntroManager : MonoBehaviour
{
    public GameObject introVideo;
    public GameObject uiCanvas;
    public VideoPlayer video;
    
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
            StartCoroutine(WaitForIntro());
            SaveOpeningSeen();
        }
    }

    private void SaveOpeningSeen()
    {
        seenOpening = true;

        PlayerPrefs.SetInt("LaunchedBefore", 1);
    }

    IEnumerator WaitForIntro()
    {
        yield return new WaitForSeconds((float)video.length);
        
        uiCanvas.SetActive(true);
        introVideo.SetActive(false);
    }

    [ContextMenu("ResetCheck")]
    void ResetCheck() => PlayerPrefs.SetInt("LaunchedBefore", 0);
    
    [ContextMenu("PassCheck")]
    void PassCheck() => PlayerPrefs.SetInt("LaunchedBefore", 1);
}
