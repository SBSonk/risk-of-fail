using UnityEngine;

public class EnableSpawning : MonoBehaviour
{
    public Room room;
    private bool active = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || !active) return;

        room.ToggleSpawns(true);
        active = false;
    }
}