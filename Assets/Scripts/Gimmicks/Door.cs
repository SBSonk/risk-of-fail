using UnityEngine;
using System.Collections;
using Pathfinding;

public class Door : MonoBehaviour
{
    public float openDelay = 0.5f;
    public float closeDelay = 0f;
    public float lerpVal = 0.25f;

    float desiredRot = 0;
    float baseRot;

    public float openRot = 90;
    public bool opened;

    Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        baseRot = transform.rotation.eulerAngles.z;

        AstarPath.active.UpdateGraphs(col.bounds, 1);
    }

    public void ToggleDoor(bool val)
    {
        float delay = val ? closeDelay : openDelay;
        if (closeDelay < 0.1f) delay = 0.1f;

        StartCoroutine(Toggle(val, delay));
    }

    public void Update()
    {
        desiredRot = opened ? 0 : openRot;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.LerpAngle(transform.rotation.eulerAngles.z, baseRot + desiredRot, lerpVal)));
    }

    
    IEnumerator Toggle(bool val, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        opened = val;

        AstarPath.active.UpdateGraphs(col.bounds, 1);
    }
}
