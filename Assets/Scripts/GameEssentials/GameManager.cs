using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameData pData;
    public Settings _settings;

    public static GameManager main;

    // Events
    public delegate void OnWeaponReceive();
    public static event OnWeaponReceive onWeaponReceive;

    void Awake()
    {
        //Debug.LogError("s");
        // Destroy duplicates
        main = this;

        // TODO: Create json file to store settings

        // TODO: Load settings from json file // TODO MAKE SETTINGS FILE
        _settings = new Settings(75);

        LoadSettings(_settings);

        // TODO: Save/load player

        Enemy.globalEnemyHealthScale = 1;
    }

    public static void GiveWeapon(Weapon weapon)
    {
        var invWep = new InventoryWeapon(weapon, 0, 0);
        invWep.Initialize();

        main.pData.weaponsOwned.Add(invWep);
        if (onWeaponReceive != null) onWeaponReceive.Invoke();
    }

    public static void GiveWeaponZombies(Weapon weapon)
    {
        var invWep = new InventoryWeapon(weapon, 0, 0);
        invWep.Initialize();

        if (main.pData.weaponsOwned.Count == 1)
        {
            main.pData.weaponsOwned.Add(invWep);
        }
        else
        {
            main.pData.weaponsOwned.Remove(PlayerStatus.player.pShooting.GetHeldWeapon());
            main.pData.weaponsOwned.Add(invWep);
        }
        
        if (onWeaponReceive != null) onWeaponReceive.Invoke();
    }
    
    public static bool CheckIfWeaponOwned(Weapon type)
    {
        foreach (InventoryWeapon w in main.pData.weaponsOwned)
        {
            if (w.weapon.name == type.name) return true;
        }
        
        return false;
    }

    // TODO: 
    public void LoadSettings(Settings settings)
    {
        Application.targetFrameRate = settings.targetFPS;
    }
}
