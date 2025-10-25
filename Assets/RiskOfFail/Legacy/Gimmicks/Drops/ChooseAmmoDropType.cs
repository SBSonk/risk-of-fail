using System.Collections.Generic;
using RiskOfFail.Combat;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChooseAmmoDropType : MonoBehaviour
{
    //public DropObject drop;
    [SerializeField] private List<Gun> drops;
    private AmmoPickup pickup;

    public void InitializeAmmo()
    {
        pickup = GetComponent<AmmoPickup>();

        // Remove drops that the player doesn't own
        var gunsOwned = new List<Gun>();
        foreach (var d in drops)
            if (PlayerStatus.player.pShooting.CheckIfWeaponOwned(d))
                gunsOwned.Add(d);

        // Choose drop to drop
        print(gunsOwned);

        if (gunsOwned.Count == 1)
            pickup.drop = gunsOwned[0].ammoDrop;
        else if (gunsOwned.Count > 1) pickup.drop = gunsOwned[Random.Range(0, gunsOwned.Count)].ammoDrop;
    }
}