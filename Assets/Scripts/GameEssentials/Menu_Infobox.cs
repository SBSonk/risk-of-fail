using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Infobox : MonoBehaviour
{
    [SerializeField] private GameObject info, tutorial;
    
    [SerializeField] private Image levelPreview;
    [SerializeField] private TextMeshProUGUI levelDescription, highScore, bestTime;

    public void SetLevel(LevelSelectItem level)
    {
        levelDescription.SetText(level.description);
        levelPreview.sprite = level.preview;
        
        highScore.SetText(LevelStats.GetHighScore(level.buildIndex).ToString("00000"));
        
        TimeSpan t = TimeSpan.FromSeconds(LevelStats.GetBestTime(level.buildIndex));
        bestTime.SetText($"{t.Minutes.ToString("00")}:{t.Seconds.ToString("00")}");
    }

    public void ShowLevel()
    {
        info.SetActive(true);
        tutorial.SetActive(false);
    }

    public void ShowTutorial()
    {
        info.SetActive(false);
        tutorial.SetActive(true);
    }
}
