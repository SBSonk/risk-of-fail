using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionActiveRoom : MonoBehaviour
{
    [SerializeField] Room lastRoom, newRoom;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        lastRoom.ToggleRoom(false);
        newRoom.ToggleRoom(true);
        Spawning2.active = newRoom.spawner;
    }
}
