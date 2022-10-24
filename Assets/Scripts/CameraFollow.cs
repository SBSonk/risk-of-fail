using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    [SerializeField] float lerpVal = 0.15f;

    [Header("RoomTransfer")]
    [SerializeField] CameraBounds currentBoundsX, currentBoundsY;

    Vector3 desiredPos;
    Rigidbody2D player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        desiredPos = (Vector3) player.position + offset;

        transform.position = Vector3.Lerp(transform.position, desiredPos, lerpVal);
    }

    public void ChangeRoomBounds(CameraBounds x, CameraBounds y)
    {
        currentBoundsX = x; 
        currentBoundsY = y;
    }
}

[System.Serializable]
public struct CameraBounds
{
    public float min, max;
}
