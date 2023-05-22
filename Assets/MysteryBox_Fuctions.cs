using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBox_Fuctions : MonoBehaviour
{   
    public Animator animator;
    public Weapon[] MgaWeapon;
    public void Buybox()
    {
        animator.Play("BoxOpen", 0, 0);

        List<Weapon> list = new List<Weapon>();
        for(int i = 0; i < MgaWeapon.Length; i++)
        {
            if (!GameManager.CheckIfWeaponOwned(MgaWeapon[i]))
            {
                list.Add(MgaWeapon[i]);
            }
        }
        int gacha = Random.Range(0, list.Count);
        print("you got weapon! WOW" + gacha);
        Weapon selected = list[gacha];
        GameManager.GiveWeaponZombies(selected);

    }


}
