using UnityEngine;
using UnityEngine.Events;

public class LootBox : InteractBase
{
    public Drops[] drops;

    public UnityEvent LootboxOpen;

    protected override void PlayerInteract()
    {
        foreach (Drops d in drops)
        {
            d.Spawn(transform.position);
        }

        LootboxOpen?.Invoke();

        Destroy(gameObject);
    }
}
