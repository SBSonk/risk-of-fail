using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool paused;
    public GameData pData;
    public Settings _settings;

    public static GameManager main;

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

    public delegate void OnWeaponReceive();
    public static event OnWeaponReceive onWeaponReceive;

    void Awake()
    {
        // Destroy duplicates
        if (!main) main = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        // TODO: Create json file to store settings

        // TODO: Load settings from json file // TODO MAKE SETTINGS FILE
        _settings = new Settings(75);

        LoadSettings(_settings);

        // TODO: Save/load player
    }

    // Calculates score given for enemy deaths
    public static void GiveScore(int baseAmount, float secondsBeforeDeath)
    {
        main.pData.fPoints += baseAmount;

        // Bonus points for killing early
        if (secondsBeforeDeath <= main.bonusKillTime) main.pData.fPoints += main.bonusPoints;

        // Calculate killstreaks
        float timeSinceLastKill = Time.time - main.lastKillTime;
        if (timeSinceLastKill <= main.killStreakTimer)
        {
            // Only give playerData.fudgePoints if killstreak is started
            if (main.currentStreak >= main.startStreakAmount)
            {
                main.pData.fPoints += main.streakPoints * (main.currentStreak - main.startStreakAmount);
            }

            if (main.currentStreak < (main.maxStreak + main.startStreakAmount)) main.currentStreak++;
        }
        else
        {
            // Remove killstreak
            main.currentStreak = 0;
        }

        main.lastKillTime = Time.time;
        if (onEnemyKilled != null) onEnemyKilled.Invoke();
    }
    // Sets the score
    public static void SetScore(int amount)
    {
        main.pData.fPoints = amount;
    }
    // Adds weapon to inventory
    public static void GiveWeapon(Weapon weapon)
    {
        var invWep = new inventoryWeapon(weapon, 0, 0);
        invWep.Initialize();

        main.pData.weaponsOwned.Add(invWep);
        if (onWeaponReceive != null) onWeaponReceive.Invoke();
    }
    // Returns true if the weapon exists in the inventory
    public static bool CheckIfWeaponOwned(Weapon type)
    {
        foreach (inventoryWeapon w in main.pData.weaponsOwned)
        {
            if (w.weapon.weaponName == type.weaponName) return true;
        }
        
        return false;
    }

    [ContextMenu("Pause")]
    public void TogglePause()
    {
        paused = !paused;

        if (paused) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    // TODO: 
    public void LoadSettings(Settings settings)
    {
        Application.targetFrameRate = settings.targetFPS;
    }
}
