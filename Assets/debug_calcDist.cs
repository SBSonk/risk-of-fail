using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debug_calcDist : MonoBehaviour
{
    public Transform from, to;

    [ContextMenu("GetDIST")]
    private void GETDIST()
    {
        print(Vector3.Distance(from.position, to.position));
    }
}
