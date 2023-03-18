using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Buyable : InteractBase
{
    public int price = 250;
    public UnityEvent OnPurchase;
    
    protected override void PlayerInteract()
    {
        // Check for points
        if (LevelStats.main.points >= price)
        {
            LevelStats.main.GiveScore(-price);
            OnPurchase?.Invoke();
        }
    }
}
