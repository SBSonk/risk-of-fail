using System;
using System.Collections;
using UnityEngine;

public class TrackingRound : Projectile
{
    Transform target;
    [SerializeField] private float rotationSpeed = 50;
    [SerializeField] private float trackingSpeed = 50, propelSpeed = 50;
    [SerializeField] private float trackingTime = 5, waitTime = 1;
    private bool tracking = true, propelled;

    public void SetTarget(Transform t) => target = t;
    
    private void Start()
    {
        StartCoroutine(TrackingTimer());
    }

    protected override void Update()
    {
        if (!active) return;
        
        Vector3 targetDir = (target.position - transform.position + new Vector3(0, 0.5f)).normalized;
        
        if (tracking)
        {
            /*rb.AddForce(transform.right * (trackingSpeed * Time.deltaTime));*/
            rb.velocity = transform.right * trackingSpeed;
            transform.right = Vector3.Lerp(transform.right, targetDir, Time.deltaTime * rotationSpeed);
        }

        if (propelled)
        {
            rb.AddForce(transform.right * (propelSpeed * Time.deltaTime));
        }
    }

    IEnumerator TrackingTimer()
    {
        yield return new WaitForSeconds(trackingTime);

        tracking = false;

        yield return new WaitForSeconds(waitTime);

        propelled = true;
    }
}