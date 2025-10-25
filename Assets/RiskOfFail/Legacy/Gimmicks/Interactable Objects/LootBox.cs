using UnityEngine.Events;

public class LootBox : InteractBase
{
    public Drops[] drops;

    public UnityEvent LootboxOpen;

    protected override void PlayerInteract()
    {
        foreach (var d in drops) d.Spawn(transform.position, out _, playerTransform.position);

        LootboxOpen?.Invoke();

        Destroy(gameObject);
    }
}