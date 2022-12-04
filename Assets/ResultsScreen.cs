using UnityEngine;
using TMPro;
using System;

public class ResultsScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI score, bulletsShot, bulletsHit, accuracy, time, grade;

    public void ShowResults()
    {
        score.text = LevelStats.main.points.ToString("00000");

        bulletsShot.text = LevelStats.main.bulletsShot.ToString();
        bulletsHit.text = LevelStats.main.bulletsHit.ToString();
        accuracy.text = LevelStats.main.GetAccuracy().ToString("00.00") + "%";

        TimeSpan t = TimeSpan.FromSeconds(LevelStats.main.time);
        time.text = $"{t.Minutes.ToString("00")}:{t.Seconds.ToString("00")}";
        grade.text = CalculateGrade(LevelStats.main.points);
    }

    string CalculateGrade(int score)
    {
        return "A";
    }
}
