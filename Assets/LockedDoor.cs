using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : Door
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent<Key>(out var key))
        {
            if (key.doorToOpen == this)
            {
                Destroy(col.gameObject);
                ToggleDoor(true);
            }
        }
    }
}
