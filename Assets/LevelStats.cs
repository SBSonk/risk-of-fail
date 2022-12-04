using UnityEngine;
using UnityEngine.Events;

public class LevelStats : MonoBehaviour
{
    public static LevelStats main;

    [Header("Kill Bonus Points")]
    [SerializeField] float bonusKillTime = 5f;
    [SerializeField] int bonusPoints = 500;

    [Header("Kill Streak Points")]
    [SerializeField] int streakPoints, startStreakAmount = 3, maxStreak = 3;
    [SerializeField] float killStreakTimer = 1f;
    int currentStreak = 0;
    float lastKillTime;

    public float time;
    public int points, enemiesKilled;

    public int bulletsShot, bulletsHit;

    private void Awake()
    {
        if (!main) main = this;
    }

    private void Start()
    {
        PlayerStatus.player.pShooting.OnShoot.AddListener(BulletShot);
    }

    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
    }

    public float GetAccuracy()
    {
        return ((float)bulletsHit / bulletsShot) * 100;
    }

    // Calculates score accounting for killstreaks
    public void GiveScore(int baseAmount, float secondsBeforeDeath)
    {
        points += baseAmount;

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
    }
    public void SetScore(int amount)
    {
        points = amount;
    }

    // Stat counter functions
    public void BulletShot()
    {
        bulletsShot++;
    }
    public void BulletHit()
    {
        bulletsHit++;
    }
    public void EnemyKilled()
    {
        enemiesKilled++;
    }
}
