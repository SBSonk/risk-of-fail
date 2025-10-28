using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 5f;
    [SerializeField] private GameObject target;
    void Start()
    {
        if (!target) target = gameObject;
        Destroy(target, timeToDestroy);
    }
}
