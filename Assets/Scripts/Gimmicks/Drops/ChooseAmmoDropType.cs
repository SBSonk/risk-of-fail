using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class ChooseAmmoDropType : MonoBehaviour
{
    AmmoPickup pickup;
    //public DropObject drop;
    [SerializeField] List<Gun> drops;

    public void InitializeAmmo()
    {
        pickup = GetComponent<AmmoPickup>();

        // Remove drops that the player doesn't own
        List<Gun> gunsOwned = new List<Gun>();
        foreach(Gun d in drops)
        {
            if (GameManager.CheckIfWeaponOwned(d)) gunsOwned.Add(d);
        }
        
        // Choose drop to drop
        if (gunsOwned.Count == 1)
        {
            pickup.drop = gunsOwned[0].ammoDrop;
        }
        else if (gunsOwned.Count > 1)
        {
            pickup.drop = gunsOwned[Random.Range(0, gunsOwned.Count)].ammoDrop;
        }
    }
}
