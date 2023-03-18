using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIncrease : MonoBehaviour
{
    public void Purchase()
    {
        PlayerStatus.player.health = 100;
        PlayerStatus.player.maxHealth = 100;
    }
}
