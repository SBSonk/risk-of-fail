using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Color active = Color.green, disabled = Color.red;

    public bool isActive, interactable = true, playerInRadius = false;

    public UnityEvent OnSwitchOn, OnSwitchToggled, OnSwitchOff;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || !interactable) return;

        playerInRadius = true;     
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || !interactable) return;
        playerInRadius = false;      
    }

    private void Update()
    {
        if (InputManager.interact && playerInRadius)
        {
            Toggle();
        }
    }

    void Toggle()
    {
        OnSwitchToggled?.Invoke();

        isActive = !isActive;

        if (isActive) OnSwitchOn?.Invoke();
        else OnSwitchOff?.Invoke();

        sprite.color = isActive ? active : disabled;
    }
}
