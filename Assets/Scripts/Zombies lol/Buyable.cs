using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Buyable : InteractBase
{
    public int price = 250;
    public bool repeatable = true;
    public UnityEvent OnPurchase;
    public ShowInteractBuyable interactIcon;
    
    protected override void PlayerInteract()
    {
        // Check for points
        if (LevelStats.main.points >= price)
        {
            LevelStats.main.GiveScore(-price);
            OnPurchase?.Invoke();

            if (!repeatable)
            {
                interactable = false;
                interactIcon.enabled = false;
            }
        }
    }
}
