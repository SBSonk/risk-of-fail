using UnityEngine;
using UnityEngine.Events;

public class RoomKillCounter : MonoBehaviour
{
    public UnityEvent OnCounterComplete;
    public int killsNeeded = 10;
    public bool active = true;

    public void AddKill()
    {
        if (!active) return;

        killsNeeded--;

        if (killsNeeded == 0)
        {
            FinishCounter();
        }
    }

    public void FinishCounter()
    {
        OnCounterComplete?.Invoke();
        active = false;
    }
}
