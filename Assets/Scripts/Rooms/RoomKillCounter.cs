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
        print("add");

        if (killsNeeded == 0)
        {
            OnCounterComplete?.Invoke();print("fin");
            active = false;
        }
    }
}
