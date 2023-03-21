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

    [Header("Kill Streak Points")]
    [SerializeField] int streakPoints;
    [SerializeField] int startStreakAmount = 3, maxStreak = 3;
    [SerializeField] float killStreakTimer = 1f;
    int currentStreak = 0;
    float lastKillTime;

    [Header("Stats")]
    public float time;
    public int points, enemiesKilled;

    public int bulletsShot, bulletsHit;
    public float damageTaken;

    [Header("Bonuses")]
    public int sharpShooterBonus = 2000;
    public int noHitBonus = 5000;

    private void Awake()
    {
        if (!main) main = this;
    }

    private void Start()
    {
        PlayerStatus.player.pShooting.OnShoot.AddListener(BulletShot);
        PlayerStatus.player.onHit.AddListener(GiveDamage);
    }

    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
    }

    public float GetAccuracy()
    {
        return ((float)bulletsHit / bulletsShot) * 100;
    }
    public Sprite GetLevelScore()
    {
        Sprite sprite = null;

        foreach (Grades g in grades.grades)
        {
            if (points >= g.scoreNeeded)
            {
                sprite = g.sprite;
            }
        }

        return sprite;
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

    public void GiveScore(int baseAmount, float secondsBeforeDeath)
    {
        points += Mathf.RoundToInt( baseAmount * pointsMultiplier);

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

    public void GiveScore(int baseAmount)
    {
        points += Mathf.RoundToInt( baseAmount * pointsMultiplier);
    } // Raw points

    public void SetScore(int amount)
    {
        points = amount;
    }

    public void GiveDamage(float damage)
    {
        damageTaken += damage;
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
