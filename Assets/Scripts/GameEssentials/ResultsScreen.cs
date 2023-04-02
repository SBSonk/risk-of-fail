using UnityEngine;
using TMPro;
using System;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ResultsScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score, bulletsShot, bulletsHit, accuracy, time, damageTaken, damageGiven, healthRestored;
    [SerializeField] private TextMeshProUGUI enemiesKilled, passedText;
    [SerializeField] Image grade;

    [SerializeField] private Animator anim;

    public void ShowResults() => ShowResults(KillFlag.LevelPassed);
    
    public void ShowResults(KillFlag flag = KillFlag.LevelPassed)
    {
        gameObject.SetActive(true);
        
        var stats = LevelStats.main;
        stats.CalculateFinalScore();
        
        anim.Play("ResultsShow");

        
        if ((flag != KillFlag.LevelPassed))
        {
            // Switch Sprites
            switch (flag)
            {
           
            }

            grade.sprite = stats.GetLevelScore(0);
        }
        else
        {
            grade.sprite = stats.GetLevelScore(stats.points);
        }
        

        score.text = stats.points.ToString("00000");

        bulletsShot.text = stats.bulletsShot.ToString();
        bulletsHit.text = stats.bulletsHit.ToString();

        float acc = stats.GetAccuracy();
        if (acc == float.PositiveInfinity) accuracy.text = "N/A";
        else accuracy.text = acc.ToString("00.00") + "%";
        
        enemiesKilled.text = stats.enemiesKilled.ToString();

        TimeSpan t = TimeSpan.FromSeconds(stats.GetTimeCompleted());
        time.text = $"{t.Minutes.ToString("00")}:{t.Seconds.ToString("00")}";

        damageTaken.text = stats.damageTaken.ToString();
        damageGiven.text = stats.damageGiven.ToString();
        healthRestored.text = stats.healthRestored.ToString();

        

        passedText.text = flag == KillFlag.LevelPassed ? "PASSED" : "FAILED";
    }
}
