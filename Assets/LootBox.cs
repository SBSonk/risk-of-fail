using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBox : MonoBehaviour
{
    public bool active;

    public GameObject[] drops;
    bool playerInRadius;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        playerInRadius = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        playerInRadius = false;
    }

    private void Update()
    {
        if (InputManager.interact && playerInRadius)
        {
            foreach (GameObject d in drops)
            {
                var drop = Instantiate(d);
                drop.GetComponentInChildren<PickupBase>().ActivateInSeconds(0.75f);
                drop.transform.position = transform.position;

                Vector3 randDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                drop.GetComponent<Rigidbody2D>().AddForce(randDir * Random.Range(3f, 6f), ForceMode2D.Impulse);
            }

            // Play destroy animation
            Destroy(gameObject);
        }
    }
}
