using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsScreen : MonoBehaviour
{
    public static ResultsScreen instance;

    [SerializeField]
    private TextMeshProUGUI score, bulletsShot, bulletsHit, accuracy, time, damageTaken, damageGiven, healthRestored;

    [SerializeField] private TextMeshProUGUI enemiesKilled, passedText;
    [SerializeField] private Image grade;

    [SerializeField] private Animator anim, avatarAnim;

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    public void ShowResults()
    {
        ShowResults(KillFlag.LevelPassed);
    }

    public void ShowResults(KillFlag flag = KillFlag.LevelPassed)
    {
        gameObject.SetActive(true);

        var stats = LevelStats.main;
        stats.CalculateFinalScore();

        anim.Play("ResultsShow");


        if (flag != KillFlag.LevelPassed)
        {
            // TODO: Switch Sprites
            switch (flag)
            {
            }

            grade.sprite = stats.GetLevelScore(0);
            avatarAnim.Play("LoseAvatar");
        }
        else
        {
            grade.sprite = stats.GetLevelScore(stats.points);
            avatarAnim.Play("WinAvatar");
        }


        score.text = stats.points.ToString("00000");

        bulletsShot.text = stats.bulletsShot.ToString();
        bulletsHit.text = stats.bulletsHit.ToString();

        var acc = stats.GetAccuracy();
        if (acc == float.PositiveInfinity) accuracy.text = "N/A";
        else accuracy.text = acc.ToString("00.00") + "%";

        enemiesKilled.text = stats.enemiesKilled.ToString();

        var timeCompleted = stats.GetTimeCompleted();
        var t = TimeSpan.FromSeconds(timeCompleted);
        time.text = $"{t.Minutes.ToString("00")}:{t.Seconds.ToString("00")}";

        damageTaken.text = stats.damageTaken.ToString();
        damageGiven.text = stats.damageGiven.ToString();
        healthRestored.text = stats.healthRestored.ToString();

        passedText.text = flag == KillFlag.LevelPassed ? "PASSED" : "FAILED";

        // Set High Scores
        var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (stats.points > LevelStats.GetHighScore(sceneIndex)) LevelStats.SetHighScore(sceneIndex, stats.points);

        if (timeCompleted > LevelStats.GetBestTime(sceneIndex)) LevelStats.SetBestTime(sceneIndex, timeCompleted);
    }
}