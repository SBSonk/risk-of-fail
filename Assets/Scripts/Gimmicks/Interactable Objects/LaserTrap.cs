using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrap : MonoBehaviour
{
    [SerializeField] BoxCollider2D col;
    [SerializeField] LineRenderer line;
    [SerializeField] LayerMask layers;

    private void Awake()
    {
        //col = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        print(line.bounds.extents);
        col.offset = line.bounds.extents - (Vector3.one * 0.25f);
        col.size = line.bounds.size;
    }

    private void Update()
    {
        print("up");
        RaycastHit2D ray = Physics2D.Linecast(transform.position, transform.right * 50f, layers);
        Debug.DrawLine(transform.position, transform.right * 100f);

        line.SetPosition(0, line.transform.position);
        if (ray)
        {
            
            print(line.transform.InverseTransformVector(ray.point));
            print(ray.collider.name);
            print("down");
            line.SetPosition(1, ray.point);
            col.transform.right = line.GetPosition(1);

            col.offset = line.bounds.extents;
            col.size = line.bounds.size;
        }
    }
}
