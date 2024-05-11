using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attract : MonoBehaviour
{
    [SerializeField] float attractStrength = 50f;
    Rigidbody2D rb;
    Transform target;
//    public Collider2D _collider;

    PickupBase pickup;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pickup = GetComponentInChildren<PickupBase>();
    }

    private void Update()
    {
        if (target) rb.AddForce((target.position - transform.position).normalized * (attractStrength * Time.deltaTime));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && pickup.active)
        {
            target = collision.transform;
           // if (_collider) _collider.enabled = false;
        }
    }
}
