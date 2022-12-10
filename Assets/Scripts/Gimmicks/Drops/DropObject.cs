using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObject : MonoBehaviour
{
    public Drops[] drops;

    public void startDrop()
    {
        foreach (Drops d in drops)
        {
            d.Spawn(transform.position);
        }
    }
}