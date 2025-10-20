using System;
using System.Collections;
using UnityEngine;

public class TrackingRound : Projectile
{
    Transform target;
    [SerializeField] private float rotationSpeed = 50;
    [SerializeField] private float trackingSpeed = 50, propelSpeed = 50;
    [SerializeField] private float trackingTime = 5, waitTime = 1;
    [SerializeField] private Animator anim;
    private bool tracking = true, propelled;
    private bool attackPlayer = false;

    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        col.enabled = false;
    }

    public void SetTarget(Transform t) => target = t;
    
    public void StartTimer()
    {
        attackPlayer = true;
        StartCoroutine(TrackingTimer());
    }

    protected override void Update()
    {
        if (target && !propelled)
        {
            Vector3 targetDir = (target.position - transform.position + new Vector3(0, 0.5f)).normalized;
            float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (!active || !attackPlayer) return;
        
        if (tracking)
        {
            /*rb.AddForce(transform.right * (trackingSpeed * Time.deltaTime));*/
            rb.linearVelocity = transform.right * trackingSpeed;
        }

        if (propelled)
        {
            rb.AddForce(transform.right * (propelSpeed * Time.deltaTime));
        }
    }

    IEnumerator TrackingTimer()
    {
        anim.Play("Tracking");
        
        yield return new WaitForSeconds(0.25f);

        col.enabled = true;
        
        yield return new WaitForSeconds(trackingTime);

        tracking = false;
        anim.Play("NonTracking");

        yield return new WaitForSeconds(waitTime);
        
        propelled = true;
    }
}