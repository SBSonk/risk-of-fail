using System;
using FirstGearGames.SmoothCameraShaker;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class Switch : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Sprite active, disabled;
    public Light2D light;

    public bool isActive, interactable = true, playerInRadius = false, disableOnUse;

    public UnityEvent OnSwitchOn, OnSwitchToggled, OnSwitchOff;
    [SerializeField] private ShakeData useShake;

    private void Start()
    {
        light.color = isActive ? Color.green : Color.red;
    }

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
        if (KInputManager.GetKey("Interact").Pressed() && playerInRadius)
        {
            Toggle();

            if (disableOnUse)
            {
                interactable = false;
                playerInRadius = false;
            }
        }
    }

    void Toggle()
    {
        OnSwitchToggled?.Invoke();

        isActive = !isActive;
        light.color = isActive ? Color.green : Color.red;
        
        if (isActive) OnSwitchOn?.Invoke();
        else OnSwitchOff?.Invoke();

        sprite.sprite = isActive ? active : disabled;
        if (useShake) CameraShakerHandler.Shake(useShake);
    }
}
