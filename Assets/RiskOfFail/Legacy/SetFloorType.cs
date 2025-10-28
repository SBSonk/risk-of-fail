using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFloorType : MonoBehaviour
{
    public FloorType TileType;

    public void SetFloor()
    {
        FootstepManager.instance.SetFloor(TileType);
    }

    public void ResetFloor()
    {
        FootstepManager.instance.ResetFloor();
    }
}
