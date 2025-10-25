using UnityEngine;
using UnityEngine.Events;

public class RoomSpawnCounter : MonoBehaviour
{
    public UnityEvent OnCounterComplete;
    public int spawnsNeeded = 10;
    public bool active = true;

    public void AddSpawn()
    {
        if (!active) return;

        spawnsNeeded--;

        if (spawnsNeeded <= 0)
        {
            OnCounterComplete?.Invoke();
            active = false;
        }
    }
}