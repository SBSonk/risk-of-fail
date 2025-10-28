using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : InteractBase
{
    [Range(0, 1)]
    public float chanceToExplode = .5f;
    
    protected override void PlayerInteract()
    {
        throw new System.NotImplementedException();
    }
}
