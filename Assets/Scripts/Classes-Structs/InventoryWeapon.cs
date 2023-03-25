using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // Used to store inventory weapon values and ammo
public class InventoryWeapon
{
    public Weapon weapon;
    public int clip, pool;
    
    public InventoryWeapon(Weapon nweapon, int nclip, int npool)
    {
        weapon = nweapon;
        clip = nclip;
        pool = npool;
    }

    public void Initialize()
    {
        clip = weapon.clipSize;
        pool = weapon.defaultAmmoCount;
    }
}