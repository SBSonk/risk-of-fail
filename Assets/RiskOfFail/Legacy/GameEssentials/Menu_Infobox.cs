using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Infobox : MonoBehaviour
{
    [SerializeField] private GameObject info;

    [SerializeField] private Image levelPreview;
    [SerializeField] private TextMeshProUGUI levelDescription, highScore, bestTime;

    [SerializeField] private LevelSelectItem defaultLevel;

    private void Start()
    {
        SetLevel(defaultLevel);
    }

    public void SetLevel(LevelSelectItem level)
    {
        levelDescription.SetText(level.description);
        levelPreview.sprite = level.preview;

        highScore.SetText(LevelStats.GetHighScore(level.buildIndex).ToString("00000"));

        var t = TimeSpan.FromSeconds(LevelStats.GetBestTime(level.buildIndex));
        bestTime.SetText($"{t.Minutes.ToString("00")}:{t.Seconds.ToString("00")}");
    }

    public void ShowLevel()
    {
        info.SetActive(true);
    }

    public void ShowTutorial()
    {
        info.SetActive(true);
    }
}