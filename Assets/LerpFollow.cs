using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float followSpeed;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * followSpeed);
    }
}
