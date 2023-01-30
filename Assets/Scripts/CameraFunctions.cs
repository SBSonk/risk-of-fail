using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFunctions : MonoBehaviour
{
    public static CameraFunctions main;

    [SerializeField] Vector3 defaultOffset;
    Vector3 offset;
    [SerializeField] float lerp = 0.5f;

    private void Awake()
    {
        if (!main) main = this;
    }

    private void FixedUpdate()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, offset + new Vector3(0f, 0f, -10f), lerp);
    }
}