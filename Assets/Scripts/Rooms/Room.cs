using UnityEngine;
using UnityEngine.Events;

public class Room : MonoBehaviour
{
    [SerializeField] GameObject collision;
    public Spawning2 spawner;

    public UnityEvent OnRoomEnter;
    public bool active = false;

    public void ToggleRoom(bool val)
    {
        if (!active && val)
        {
            OnRoomEnter?.Invoke();
        }

        active = val;
    }

    public void ToggleSpawns(bool val)
    {
        if (!spawner) return;

        spawner.enabled = val;

        if (!val) spawner.CancelInvoke();
    }
}
