using System.Collections;
using System.Collections.Generic;
using RiskOfFail.Combat;
using UnityEngine;

public class MysteryBox_Fuctions : MonoBehaviour
{   
    public Animator animator;
    public Weapon[] MgaWeapon;
    public void Buybox()
    {
        animator.Play("BoxOpen", 0, 0);

        PlayerShooting shooting = PlayerStatus.instance.shooting;
        List<Weapon> list = new List<Weapon>();
        for(int i = 0; i < MgaWeapon.Length; i++)
        {
            if (!shooting.CheckIfWeaponOwned(MgaWeapon[i]))
            {
                list.Add(MgaWeapon[i]);
            }
        }
        int gacha = Random.Range(0, list.Count);
        print("you got weapon! WOW" + gacha);
        Weapon selected = list[gacha];
        
        if (!shooting.CheckIfWeaponOwned(selected))
        {
            InventoryWeapon newWeapon = new InventoryWeapon(selected, selected.clipSize, selected.defaultAmmoCount);
            if (shooting.InventoryFull()) shooting.ReplaceWeapon(newWeapon, shooting.GetHeldIndex());
            else shooting.GiveWeapon(newWeapon);
        }
        else
        {
            shooting.GiveAmmo(shooting.GetWeaponFromInventory(selected), Mathf.FloorToInt(selected.defaultAmmoCount/4f));
        }
    }


}
