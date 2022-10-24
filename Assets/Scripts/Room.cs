using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] GameObject collision;
    [SerializeField] Spawning2 spawner;

    public void ToggleRoom(bool val)
    {
        collision.SetActive(val);
    }

    public void ToggleSpawns(bool val)
    {
        spawner.enabled = val;
    }
}
