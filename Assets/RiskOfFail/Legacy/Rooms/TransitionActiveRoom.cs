using UnityEngine;

public class TransitionActiveRoom : MonoBehaviour
{
    [SerializeField] private Room lastRoom, newRoom;


    // delete this script when
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        lastRoom.ToggleRoom(false);
        newRoom.ToggleRoom(true);
        //Spawning2.active = newRoom.spawner;
    }
}