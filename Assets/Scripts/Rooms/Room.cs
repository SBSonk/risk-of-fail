using UnityEngine;
using UnityEngine.Events;

public class Room : MonoBehaviour
{
    [SerializeField] GameObject collision;
    public Spawning2 spawner;

    public UnityEvent OnRoomEnter;

    public void ToggleRoom(bool val)
    {
        if (val && collision.activeInHierarchy == false)
        {
            OnRoomEnter?.Invoke();
        }

        collision.SetActive(val);
    }

    public void ToggleSpawns(bool val)
    {
        spawner.enabled = val;

        if (!val) spawner.CancelInvoke();
    }
}
