using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutManager : MonoBehaviour
{
    public static LoadoutManager instance;

    [SerializeField] private Weapon fallbackWeapon;

    // 3 Slots by default
    [SerializeField] Weapon[] slots = new Weapon[3];

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public Weapon[] GetAllSlots() => slots;

    public Weapon GetWeapon(int index) => slots[index];
    
    public void AssignSlot(int index, Weapon w) => slots[index] = w;
    
    public bool SlotsContainMeleeWeapon()
    {
        bool contains = false;

        foreach (var weapon in slots)
        {
            if (weapon is not MeleeWeapon) continue;
            
            contains = true;
            break;
        }
        
        return contains;
    }
    
    public List<InventoryWeapon> InitializeWeaponPool()
    {
        List<InventoryWeapon> pool = new List<InventoryWeapon>();

        foreach (var weapon in slots)
        {
            if (!weapon) continue;
            
            pool.Add(new InventoryWeapon(weapon, weapon.clipSize, weapon.defaultAmmoCount));
        }

        if (pool.Count == 0) pool.Add(new InventoryWeapon(fallbackWeapon, fallbackWeapon.clipSize, fallbackWeapon.defaultAmmoCount));
        
        return pool;
    }
}