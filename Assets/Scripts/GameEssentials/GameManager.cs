using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameData pData;

    public static GameManager main;

    // Events
    public delegate void OnWeaponReceive();
    public static event OnWeaponReceive onWeaponReceive;

    void Awake()
    {
        // Destroy duplicates
        main = this;
        
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
}
