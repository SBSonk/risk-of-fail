using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObject : MonoBehaviour
{
    
    public Drops[] drops;

    public void startDrop()
    {
        foreach (Drops d in drops)
        {
            // Ignore if drop chance fails
            if ((d.dropChance / 100f) < Random.value) return;

            // Decide how much to drop
            int amountToDrop = Random.Range(d.minDropAmount, d.dropAmount);

            // Drop object/s
            for (int i = 0; i < amountToDrop; i++)
            {
                // Offset spawn location
                Vector3 spawn = transform.position;

                // Spawn item
                Rigidbody2D rb = Instantiate(d.obj, spawn, Quaternion.identity).GetComponent<Rigidbody2D>();

                // Make them shoot out around the spawn point
                Vector3 randDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                rb.AddForce(randDir * Random.Range(2f, 6f), ForceMode2D.Impulse);
            }
        }
    }
}

[System.Serializable]
public struct Drops
{
    public GameObject obj;
    [Range(0, 100)]
    public int dropChance;
    public int dropAmount, minDropAmount;
}