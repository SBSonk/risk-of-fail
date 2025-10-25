using System.Collections;
using RiskOfFail.Combat;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public Rigidbody2D bulletPrefab;
    public float bulletVel, radius;
    public float timeBetweenBursts, firerate;
    public int burstAmount = 3;
    public float rotateSpeed = 0.25f;
    public bool canShoot = true, targetAcquired;

    public LayerMask targetLayer;
    private Transform nearest;
    private float nextBurst;

    private void Update()
    {
        if (Time.time >= nextBurst) canShoot = true;

        if (canShoot && targetAcquired)
        {
            StartCoroutine(Burst());
            nextBurst = Time.time + timeBetweenBursts;

            canShoot = false;
        }
    }

    private void FixedUpdate()
    {
        var col = Physics2D.OverlapCircleAll(transform.position, radius);

        for (var i = 0; i < col.Length; i++)
        {
            if (!col[i].GetComponent<Enemy>() || col[i].GetComponent<PlayerShooting>()) continue;

            if (nearest == null)
            {
                nearest = col[i].transform;
                continue;
            }

            var dist = Vector3.Distance(col[i].transform.position, transform.position);
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

    private IEnumerator Burst()
    {
        for (var i = 0; i < burstAmount; i++)
        {
            if (!nearest) continue;

            var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.AddForce(bullet.transform.up * bulletVel, ForceMode2D.Impulse);

            yield return new WaitForSeconds(firerate);
        }

        yield return null;
    }
}