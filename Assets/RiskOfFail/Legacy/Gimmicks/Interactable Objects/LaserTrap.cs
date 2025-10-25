using UnityEngine;

public class LaserTrap : MonoBehaviour
{
    [SerializeField] private BoxCollider2D col;
    [SerializeField] private LineRenderer line;
    [SerializeField] private LayerMask layers;

    private void Awake()
    {
        //col = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        print(line.bounds.extents);
        col.offset = line.bounds.extents - Vector3.one * 0.25f;
        col.size = line.bounds.size;
    }

    private void Update()
    {
        print("up");
        var ray = Physics2D.Linecast(transform.position, transform.right * 50f, layers);
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