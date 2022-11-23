using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{
    public SpriteRenderer sprite, outline, interactIcon;
    public Color active = Color.green, disabled = Color.red;

    public bool isActive, interactable = true, playerInRadius = false;

    public UnityEvent OnSwitchOn, OnSwitchToggled, OnSwitchOff;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || !interactable) return;

        playerInRadius = true;
        StopAllCoroutines();
        StartCoroutine(SprFunctions.Fade(outline, outline.color, Color.white, 0.25f));
        StartCoroutine(SprFunctions.Fade(interactIcon, interactIcon.color, Color.white, 0.25f));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || !interactable) return;

        playerInRadius = false;
        StopAllCoroutines();
        StartCoroutine(SprFunctions.Fade(outline, outline.color, Color.clear, 0.25f));
        StartCoroutine(SprFunctions.Fade(interactIcon, interactIcon.color, Color.clear, 0.25f));
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
