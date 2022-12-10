using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class ResultsScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI score, bulletsShot, bulletsHit, accuracy, time, dmg;
    [SerializeField] Image grade;

    public void ShowResults()
    {
        var stats = LevelStats.main;
        stats.CalculateFinalScore();

        score.text = stats.points.ToString("00000");

        bulletsShot.text = stats.bulletsShot.ToString();
        bulletsHit.text = stats.bulletsHit.ToString();
        accuracy.text = stats.GetAccuracy().ToString("00.00") + "%";

        TimeSpan t = TimeSpan.FromSeconds(stats.time);
        time.text = $"{t.Minutes.ToString("00")}:{t.Seconds.ToString("00")}";

        dmg.text = stats.damageTaken.ToString("00000");

        grade.sprite = stats.GetLevelScore();
    }
}
