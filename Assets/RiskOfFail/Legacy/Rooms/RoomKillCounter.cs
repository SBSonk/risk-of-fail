using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RoomKillCounter : MonoBehaviour
{
    public UnityEvent OnCounterComplete;
    public int killsNeeded = 10;
    public bool active = true;

    public TextMeshPro text;
    private int startKillsAmountNeeded;

    public void Reset()
    {
        killsNeeded = startKillsAmountNeeded;
        active = true;
    }

    private void Start()
    {
        UpdateText();

        startKillsAmountNeeded = killsNeeded;
    }

    [ContextMenu("AddKill")]
    public void AddKill()
    {
        if (!active) return;

        killsNeeded--;

        if (killsNeeded == 0) FinishCounter();

        UpdateText();
    }

    private void UpdateText()
    {
        if (text) text.text = killsNeeded.ToString();
    }

    public void FinishCounter()
    {
        active = false;
        OnCounterComplete?.Invoke();
    }
}