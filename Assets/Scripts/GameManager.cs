using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager main;

    public int score;

    [SerializeField] int targetFPS = 60;

    [Header("Kill Bonus Points")]
    [SerializeField] float bonusKillTime = 5f;
    [SerializeField] int bonusPoints = 500;

    [Header("Kill Streak Points")]
    [SerializeField] int streakPoints, startStreakAmount = 3, maxStreak = 3;
    [SerializeField] float killStreakTimer = 1f;
    int currentStreak = 0;
    float lastKillTime;

    // Events
    public delegate void OnEnemyKill();
    public static event OnEnemyKill onEnemyKilled;

    void Start()
    {
        if (main == null) main = this;

        // TODO: Create json file to store settings

        // TODO: Load settings from json file

        Application.targetFrameRate = targetFPS;
    }

    public void GiveScore(int baseAmount, float secondsBeforeDeath)
    {
        if (onEnemyKilled != null) onEnemyKilled.Invoke();

        score += baseAmount;

        // Bonus points for killing early
        if (secondsBeforeDeath <= bonusKillTime) score += bonusPoints;

        // Calculate killstreaks
        float timeSinceLastKill = Time.time - lastKillTime;
        if (timeSinceLastKill <= killStreakTimer)
        {
            // Only give score if killstreak is started
            if (currentStreak >= startStreakAmount)
            {
                score += streakPoints * (currentStreak - startStreakAmount);
            }

            if (currentStreak < (maxStreak + startStreakAmount)) currentStreak++;
        }
        else
        {
            // Remove killstreak
            currentStreak = 0;
        }

        lastKillTime = Time.time;
    }

    [ContextMenu("Pause")]
    public void Pause()
    {
        Time.timeScale = 0;
    }

    [ContextMenu("Unpause")]
    public void Unpause()
    {
        Time.timeScale = 1;
    }
}
