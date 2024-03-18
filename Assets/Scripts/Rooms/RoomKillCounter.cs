using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RoomKillCounter : MonoBehaviour
{
    public UnityEvent OnCounterComplete;
    public int killsNeeded = 10;
    public bool active = true;

    public TextMeshPro text;
    
    public void AddKill()
    {
        if (!active) return;

        killsNeeded--;

        if (killsNeeded == 0)
        {
            FinishCounter();
        }

        if (text)text.text = (killsNeeded/2).ToString();
    }

    public void FinishCounter()
    {
        OnCounterComplete?.Invoke();
        active = false;
        
        print("f");
    }
}
