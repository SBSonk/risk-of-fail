using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Drone : MonoBehaviour
{
    public Rigidbody2D bulletPrefab;
    public float bulletVel, radius;
    public float timeBetweenBursts, firerate;
    public int burstAmount = 3;
    public float rotateSpeed = 0.25f;
    public bool canShoot = true, targetAcquired;
    float nextBurst;

    public LayerMask targetLayer;
    Transform nearest;

    private void Update()
    {
        if (Time.time >= nextBurst)
        {
            canShoot = true;
        }

        if (canShoot && targetAcquired)
        {
            StartCoroutine(Burst());
            nextBurst = Time.time + timeBetweenBursts;

            canShoot = false;
        }
    }

    private void FixedUpdate()
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, radius);

        for (int i = 0; i < col.Length; i++)
        {
            if (!col[i].GetComponent<Enemy>() || col[i].GetComponent<PlayerShooting>()) continue;

            if (nearest == null) 
            {   
                nearest = col[i].transform;
                continue;
            }

            float dist = Vector3.Distance(col[i].transform.position, transform.position);
            if (dist < Vector3.Distance(nearest.position, transform.position)) nearest = col[i].transform;
        }

        if (nearest)
        {
            transform.up = Vector3.Lerp(transform.up, nearest.position - transform.position, rotateSpeed);
            targetAcquired = true;
        }
        else
        {
            targetAcquired = false;
        }
    }

    IEnumerator Burst()
    {
        for (int i = 0; i < burstAmount; i++)
        {
            if (!nearest) continue;

            Rigidbody2D bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.AddForce(bullet.transform.up * bulletVel, ForceMode2D.Impulse);

            yield return new WaitForSeconds(firerate);
        }

        yield return null;
    }
}
