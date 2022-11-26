using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attract : MonoBehaviour
{
    [SerializeField] float attractStrength = 50f;
    Rigidbody2D rb;
    Transform target;
    public Collider2D _collider;

    PickupBase pickup;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pickup = GetComponentInChildren<PickupBase>();
    }

    private void FixedUpdate()
    {
        if (target) rb.AddForce((target.position - transform.position).normalized * attractStrength * Time.fixedDeltaTime);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && pickup.active)
        {
            target = collision.transform;
            _collider.enabled = false;
        }
    }
}
