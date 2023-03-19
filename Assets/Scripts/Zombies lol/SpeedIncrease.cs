using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedIncrease : MonoBehaviour
{
    public void Purchase()
    {
        PlayerStatus.player.pMovement.moveSpeed = 5500;
    }
}
