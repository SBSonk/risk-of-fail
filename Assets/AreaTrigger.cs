using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaTrigger : MonoBehaviour
{
    public UnityEvent OnFirstEnter, OnRoomEnter, OnRoomLeave;
    public bool active = false;
    bool unEntered = true;
    
    public float timeToRegisterInside = 1;
    float timeInside;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        timeInside = 0;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        timeInside += Time.deltaTime;
        if (timeInside >= timeToRegisterInside)
        {
            if (unEntered)
            {
                OnFirstEnter?.Invoke();
                unEntered = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        
        ToggleArea(false);
    }
    
    public void ToggleArea(bool val)
    {
        if (!active && val)
        {
            OnRoomEnter?.Invoke();
        } else if (active && !val)
        {
            OnRoomLeave?.Invoke();
        }

        active = val;
    }
}
