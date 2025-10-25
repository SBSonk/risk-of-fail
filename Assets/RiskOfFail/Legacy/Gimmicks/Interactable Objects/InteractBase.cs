using UnityEngine;
using UnityEngine.Events;

public abstract class InteractBase : MonoBehaviour
{
    public bool interactable = true, playerInRadius;
    public Transform playerTransform;
    public UnityEvent OnPlayerEnter, OnPlayerLeave, OnPickup;
    protected Animator anim;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (interactable && playerInRadius && KInputManager.GetKey("Interact").PressedDown())
        {
            OnPickup?.Invoke();
            PlayerInteract();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        playerInRadius = true;
        playerTransform = collision.transform;

        OnPlayerEnter?.Invoke();

        if (anim)
            anim.Play("InRange");
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        playerInRadius = false;
        playerTransform = null;

        OnPlayerLeave?.Invoke();

        if (anim)
            anim.Play("OutRange");
    }

    protected abstract void PlayerInteract();
}