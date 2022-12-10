using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InteractBase : MonoBehaviour
{
    public bool interactable = true, playerInRadius = false;
    public UnityEvent OnPlayerEnter, OnPlayerLeave;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        playerInRadius = true;

        OnPlayerEnter?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        playerInRadius = false;

        OnPlayerLeave?.Invoke();
    }

    private void Update()
    {
        if (interactable && playerInRadius && InputManager.interact)
        {
            PlayerInteract();
        }
    }

    protected abstract void PlayerInteract();
}
