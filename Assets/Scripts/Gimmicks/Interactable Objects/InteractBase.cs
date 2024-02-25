using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InteractBase : MonoBehaviour
{
    public bool interactable = true, playerInRadius = false;
    public Transform playerTransform;
    public UnityEvent OnPlayerEnter, OnPlayerLeave, OnPickup;
    private Animator anim;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        playerInRadius = true;
        playerTransform = collision.transform;

        OnPlayerEnter?.Invoke();
        
        if (anim)
            anim.Play("InRange");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        playerInRadius = false;
        playerTransform = null;

        OnPlayerLeave?.Invoke();
        
        if (anim)
            anim.Play("OutRange");
    }

    private void Update()
    {
        if (interactable && playerInRadius && KInputManager.GetKey("Interact").PressedDown())
        {
            PlayerInteract();
            OnPickup?.Invoke();
        }
    }

    protected abstract void PlayerInteract();
}
