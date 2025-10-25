using UnityEngine;
using UnityEngine.Events;

public class AreaTrigger : MonoBehaviour
{
    public UnityEvent OnFirstEnter, OnRoomEnter, OnRoomLeave;
    public bool active;
    public bool interactable = true, disableOnUse = true;

    public float timeToRegisterInside;
    private float timeInside;
    private bool unEntered = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        timeInside = 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        ToggleArea(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || !interactable) return;

        timeInside += Time.deltaTime;
        if (timeInside >= timeToRegisterInside)
        {
            if (unEntered)
            {
                OnFirstEnter?.Invoke();
                unEntered = false;

                if (disableOnUse) interactable = false;
            }

            ToggleArea(true);
        }
    }

    public void SetInteractive(bool val)
    {
        interactable = val;
    }

    public void ToggleArea(bool val)
    {
        if (!active && val)
            OnRoomEnter?.Invoke();
        else if (active && !val) OnRoomLeave?.Invoke();

        active = val;
    }
}