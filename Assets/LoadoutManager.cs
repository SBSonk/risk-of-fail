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
    
    public InventoryWeapon[] InitializeWeaponPool()
    {
        InventoryWeapon[] pool = new InventoryWeapon[slots.Length];

        int weapons = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null) continue;

            pool[i] = new InventoryWeapon(slots[i], slots[i].clipSize, slots[i].defaultAmmoCount);
            weapons++;
        }
        
        if (weapons == 0) pool[0] = new InventoryWeapon(fallbackWeapon, fallbackWeapon.clipSize, fallbackWeapon.defaultAmmoCount);
        
        return pool;
    }
    
    public bool CheckIfWeaponOwned(Weapon type)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null) continue;

            if (slots[i].name == type.name) return true;
        }
        
        return false;
    }
}