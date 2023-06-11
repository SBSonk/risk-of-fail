using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossCircleHandler : MonoBehaviour
{
    [SerializeField] private Alive bossCirclePrefab;
    [SerializeField] private Transform circleParent;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float timeBetweenSpawns;
    private int circlesLeft;
    
    public UnityEvent OnCirclesDestroyed;

    public void Initialize(int amountToSpawn)
    {
        // spawn all circles
        for (int i = 0; i < amountToSpawn; i++)
        {
            circlesLeft++;
        }

        StartCoroutine(SpawnSequence(amountToSpawn));
    }

    IEnumerator SpawnSequence(int amountToSpawn)
    {
        // spawn all circles
        for (int i = 0; i < amountToSpawn; i++)
        {
            Alive circle = Instantiate(bossCirclePrefab, circleParent);
            circle.onDeath.AddListener((flag =>
            {
                DestroyedCircle();
            }));

            circle.transform.position = circleParent.transform.position + (Vector3.right * 9);

            yield return new WaitForSeconds((360/rotateSpeed) / amountToSpawn);
        }
    }
    
    private void Update()
    {
        circleParent.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));
    }

    void DestroyedCircle()
    {
        circlesLeft--;

        if (circlesLeft <= 0)
        {
            OnCirclesDestroyed?.Invoke();
            Destroy(gameObject, 5f);
        }
    }
}
