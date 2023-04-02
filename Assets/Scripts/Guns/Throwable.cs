using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public abstract class Throwable : MonoBehaviour
{
    [SerializeField] protected float fuseTimer = 3;
    [SerializeField] protected float explosionRadius = 6;
    [SerializeField] protected GameObject spawnOnExplode;

    private void Start()
    {
        StartCoroutine(StartFuse());
    }

    IEnumerator StartFuse()
    {
        yield return new WaitForSeconds(fuseTimer);
        
        Explode();
        if (spawnOnExplode) Instantiate(spawnOnExplode, transform.position, quaternion.identity);
        Destroy(gameObject);
    }

    public void Detonate()
    {
        StopAllCoroutines();
        
        Explode();

        if (spawnOnExplode) Instantiate(spawnOnExplode, transform.position, quaternion.identity);
        Destroy(gameObject);
    }
    
    protected abstract void Explode();
}
