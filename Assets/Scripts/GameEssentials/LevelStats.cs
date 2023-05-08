using System;
using UnityEngine;
using UnityEngine.Events;

public class LevelStats : MonoBehaviour
{
    public static LevelStats main;

    public LevelGrades grades;

    public float pointsMultiplier = 1;
    
    [Header("Kill Bonus Points")]
    [SerializeField] float bonusKillTime = 5f;
    [SerializeField] int bonusPoints = 500;
    [SerializeField] private float meleeBonusPoints = 150f;

    [Header("Kill Streak Points")]
    [SerializeField] int streakPoints;
    [SerializeField] int startStreakAmount = 3, maxStreak = 3;
    [SerializeField] float killStreakTimer = 1f;
    int currentStreak = 0;
    float lastKillTime;

    [Header("Stats")] public int points;
    public int enemiesKilled;

    public int bulletsShot, bulletsHit;
    public float damageTaken, damageGiven, healthRestored;

    private float startTimeStamp;
    
    [Header("Bonuses")]
    public int sharpShooterBonus = 2000;
    public int noHitBonus = 5000;

    [Header("Discord RPC")] public string details;
    public string state = "Solo";

    private void Awake()
    {
        if (!main) main = this;
    }

    private void Start()
    {
        if (PlayerStatus.player)
        {
            PlayerStatus.player.pShooting.OnShoot.AddListener(BulletShot);
            PlayerStatus.player.onHit.AddListener(ReceiveDamage);
            PlayerStatus.player.onHeal.AddListener(GiveHealth);
        }

        try
        {
            DiscordRPCManager.singleton.ChangeDiscordState(details, state);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Discord not detected...");
        }

        startTimeStamp = Time.time;
    }

    public float GetAccuracy()
    {
        return ((float)bulletsHit / bulletsShot) * 100;
    }
    
    public Sprite GetLevelScore(int score)
    {
        Sprite sprite = null;

        foreach (Grades g in grades.grades)
        {
            if (score >= g.scoreNeeded)
            {
                sprite = g.sprite;
            }
        }

        return sprite;
    }

    public float GetTimeCompleted()
    {
        return Time.time - startTimeStamp;
    }
    
    public void CalculateFinalScore() // Calculate point bonuses
    {
        if (GetAccuracy() >= 90f)
        {
            print("Sharpshooter bonus!");
            GiveScore(sharpShooterBonus);
        }
        
        if (damageTaken == 0)
        {
            print("No hit bonus!");
            GiveScore(noHitBonus);
        }
    }

    public void GiveScore(int baseAmount, float secondsBeforeDeath, KillFlag flag)
    {
        points += Mathf.RoundToInt( baseAmount * pointsMultiplier);
        if (flag == KillFlag.Melee) points += Mathf.RoundToInt(meleeBonusPoints);

        // Bonus points for killing early
        if (secondsBeforeDeath <= bonusKillTime) points += bonusPoints;

        // Calculate killstreaks
        float timeSinceLastKill = Time.time - lastKillTime;
        if (timeSinceLastKill <= killStreakTimer)
        {
            // Only give playerData.fudgePoints if killstreak is started
            if (currentStreak >= startStreakAmount)
            {
                points += streakPoints * (currentStreak - startStreakAmount);
            }

            if (currentStreak < (maxStreak + startStreakAmount)) currentStreak++;
        }
        else
        {
            // Remove killstreak
            currentStreak = 0;
        }

        lastKillTime = Time.time;
    } // Applies killstreak bonuses

    public void GiveScore(int baseAmount) => points += Mathf.RoundToInt( baseAmount * pointsMultiplier); // Raw points

    public void SetScore(int amount) => points = amount;

    private void ReceiveDamage(float damage) => damageTaken += damage;

    public void GiveDamage(float damage) => damageGiven += damage;
    
    public void GiveHealth(float health) => healthRestored += health;

    // Stat counter functions
    private void BulletShot()
    {
        bulletsShot++;
    }
    public void BulletHit()
    {
        bulletsHit++;
    }
    public void EnemyKilled(KillFlag flag)
    {
        enemiesKilled++;

        switch (flag)
        {
            
        }
    }

    public static int GetHighScore(int buildIndex) => PlayerPrefs.GetInt(buildIndex + "Score", 0);
    public static void SetHighScore(int buildIndex, int score) => PlayerPrefs.SetInt(buildIndex + "Score", score);
    public static float GetBestTime(int buildIndex) => PlayerPrefs.GetFloat(buildIndex + "Time", 0);
    public static void SetBestTime(int buildIndex, float time) => PlayerPrefs.SetFloat(buildIndex + "Time", time);  
}

public enum KillFlag
{
    Melee, Ranged, Self, AreaOfEffect, LevelPassed
}
