using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class ChooseAmmoDropType : MonoBehaviour
{
    AmmoPickup pickup;
    [SerializeField] List<AmmoDrops> drops;

    public void InitializeAmmo()
    {
        pickup = GetComponent<AmmoPickup>();

        // Remove drops that the player doesn't own
        List<AmmoDrops> dropsToRemove = new List<AmmoDrops>();
        foreach(AmmoDrops d in drops)
        {
            if (!GameManager.CheckIfWeaponOwned(d.typeToGive)) dropsToRemove.Add(d);
        }

        foreach(AmmoDrops d in dropsToRemove)
        {
            drops.Remove(d);
        }

        // Choose drop to drop
        int rand = Random.Range(0, drops.Count);

        pickup.drop = drops[rand];
    }
}
