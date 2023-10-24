using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public abstract class Throwable : MonoBehaviour
{
    [SerializeField] protected float fuseTimer = 3;
    [SerializeField] protected float explosionRadius = 6;
    [SerializeField] protected GameObject spawnOnExplode;

    public Vector3 target;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float height = 1f;

    private Vector3 startPosition;
    private float distance;
    private float startTime;
    private bool isMoving = false;
    
    public void StartArc(Vector3 desiredPos)
    {
        target = desiredPos;
        startPosition = transform.position;
        distance = Vector3.Distance(startPosition, target);
        startTime = Time.time;

        isMoving = true;
    }

    private void Update()
    {
        if (!isMoving)
            return;

        // Calculate the current duration since the start
        float duration = Time.time - startTime;

        // Calculate the normalized progress between start and target
        float normalizedProgress = duration / distance * speed;

        // Apply a curve to get the arc-like motion
        float curveValue = Mathf.Sin(normalizedProgress * Mathf.PI);

        // Calculate the vertical offset using the curve and height
        float yOffset = curveValue * height;

        // Calculate the new position using lerp
        Vector3 newPosition = Vector3.Lerp(startPosition, target, normalizedProgress);

        // Apply the vertical offset to the new position
        newPosition += Vector3.up * yOffset;

        // Update the object's position
        transform.position = newPosition;

        // Check if the object has reached the target position
        if (normalizedProgress >= 1f)
        {
            isMoving = false;
            // Optional: Perform any additional actions once the object reaches the target position
            StartCoroutine(StartFuse());
        }
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
