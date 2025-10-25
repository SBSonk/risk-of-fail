using UnityEngine;

public class Attract : MonoBehaviour
{
    [SerializeField] private float attractStrength = 50f;
//    public Collider2D _collider;

    private PickupBase pickup;
    private Rigidbody2D rb;

    private Transform target;

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
        if (collision.CompareTag("Player") && pickup.active) target = collision.transform;
        // if (_collider) _collider.enabled = false;
    }
}