using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIncrease : MonoBehaviour
{
    public Sprite perkIcon;
    
    public void Purchase()
    {
        Effect();
        
        GetComponent<Animator>().Play("Bought");
        PerkSlots.main.AddIcon(perkIcon);
    }

    protected virtual void Effect()
    {
        PlayerStatus.player.health = 100;
        PlayerStatus.player.maxHealth = 100;
    }
}
