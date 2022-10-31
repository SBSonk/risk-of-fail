using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow cam;

    [SerializeField] public Vector3 offset;
    [SerializeField] float lerpVal = 0.15f, roomViewLerp = 0.05f;

    [Header("RoomTransfer")]
    [SerializeField] CameraBounds worldBoundsX, worldBoundsY;

    public float cameraSize;
    public bool lockToCenter;
    public Vector3 cameraCenter;
    Vector3 desiredPos;
    Rigidbody2D player;

    new Camera camera;
    float startCameraSize;
    Vector3 startOffset;

    private void Awake()
    {
        cam = this;
        player = GameObject.Find("Player").GetComponent<Rigidbody2D>();

        camera = Camera.main;
        cameraSize = camera.orthographicSize;
        startCameraSize = cameraSize;
        startOffset = offset;
    }

    private void FixedUpdate()
    {
        float lValue = lockToCenter ? roomViewLerp : lerpVal;

        if (lockToCenter)
        {
            desiredPos = cameraCenter + offset;
        } else
        {
            desiredPos = (Vector3)player.position + offset;
            desiredPos.x = Mathf.Clamp(desiredPos.x, worldBoundsX.min, worldBoundsX.max);
            desiredPos.y = Mathf.Clamp(desiredPos.y, worldBoundsY.min, worldBoundsY.max);
        }
    
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, cameraSize, lerpVal);
        transform.position = Vector3.Lerp(transform.position, desiredPos, lValue);
    }

    public void ChangeRoomBounds(CameraBounds x, CameraBounds y)
    {
        worldBoundsX = x;
        worldBoundsY = y;
    }

    public void ResetCameraSize()
    {
        cameraSize = startCameraSize;
        offset = startOffset;
    }
}

[System.Serializable]
public struct CameraBounds
{
    public float min, max;
}
