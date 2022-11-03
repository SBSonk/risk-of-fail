using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow cam;

    [SerializeField] public Vector3 offset;
    [SerializeField] float lerpVal = 0.15f, roomViewLerp = 0.05f;

    [Header("RoomTransfer")]
    [SerializeField] CameraBounds worldBoundsX, worldBoundsY;
    public CameraBounds currentBoundsX, currentBoundsY;

    public float cameraSize;
    public bool lockToCenter;
    public Transform cameraCenter;
    Vector3 desiredPos, lastPlayerVel;
    Rigidbody2D player;

    new Camera camera;
    float startCameraSize;
    Vector3 startOffset;
    CameraBounds startX, startY;

    private void Awake()
    {
        cam = this;
        player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        camera = Camera.main;  
    }

    private void Start()
    {
        cameraSize = camera.orthographicSize;
        startCameraSize = cameraSize;
        startOffset = offset;

        currentBoundsX = worldBoundsX;
        currentBoundsY = worldBoundsY;
        startX = worldBoundsX;
        startY = worldBoundsY;
    }

    private void FixedUpdate()
    {
        float lValue = lockToCenter ? roomViewLerp : lerpVal;

        if (lockToCenter)
        {
            desiredPos = cameraCenter.position + cameraCenter.InverseTransformPoint(player.position);
        } else
        {
            desiredPos = (Vector3)player.position;
        }

        desiredPos += lastPlayerVel;
        desiredPos.x = Mathf.Clamp(desiredPos.x, worldBoundsX.min, worldBoundsX.max);
        desiredPos.y = Mathf.Clamp(desiredPos.y, worldBoundsY.min, worldBoundsY.max);
        desiredPos += offset;

        if (player.velocity.magnitude > 0) lastPlayerVel = Vector3.Lerp(lastPlayerVel, player.velocity.normalized, lValue);

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

    public void ResetCameraBounds()
    {
        worldBoundsX = startX;
        worldBoundsY = startY;
    }
}   