using RiskOfFail.Combat;
using UnityEngine;

public class HoverFollow : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 2500, avoidanceStrength = 200;
    public float minDistance = 26, minAvoid = 20;
    private LayerMask helicopter;
    private Rigidbody2D rb;

    private void Awake()
    {
        target = FindObjectOfType<PlayerMovement>().transform;
        rb = GetComponent<Rigidbody2D>();
        helicopter = LayerMask.NameToLayer("Helicopter");
    }

    private void FixedUpdate()
    {
        if (!PlayerStatus.instance) return;

        var distanceToPlayer = Vector3.Distance(transform.position, target.position);
        if (distanceToPlayer <= minAvoid)
            rb.AddForce((transform.position - target.position).normalized * moveSpeed * Time.fixedDeltaTime);
        else if (distanceToPlayer >= minDistance)
            rb.AddForce((target.position - transform.position).normalized * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer != helicopter) return;

        rb.AddForce(-(collision.transform.position - transform.position).normalized * avoidanceStrength *
                    Time.fixedDeltaTime);
    }
}