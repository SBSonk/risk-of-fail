using UnityEngine;

public class DropObject : MonoBehaviour
{
    public Drops[] drops;

    public void startDrop()
    {
        foreach (var d in drops) d.Spawn(transform.position);
    }
}