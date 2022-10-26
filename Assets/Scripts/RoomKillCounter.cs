using UnityEngine;
using UnityEngine.Events;

public class RoomKillCounter : MonoBehaviour
{
    public UnityEvent OnCounterComplete;
    public int killsNeeded;
    public bool active = true;

    public void AddKill()
    {
        if (!active) return;

        killsNeeded--;

        if (killsNeeded == 0)
        {
            OnCounterComplete?.Invoke();
            active = false;
        }
    }
}
