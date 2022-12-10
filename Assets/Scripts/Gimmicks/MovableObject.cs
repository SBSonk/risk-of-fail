using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [SerializeField] bool canBePushed = false;
    Rigidbody2D rb;

    RigidbodyConstraints2D defaultConstraints;

    public void SetPush(bool val)
    {
        canBePushed = val;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        defaultConstraints = rb.constraints;
    }

    private void Update()
    {
        if (!canBePushed)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            rb.constraints = defaultConstraints;
        }
    }
}
