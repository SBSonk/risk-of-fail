using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Room : MonoBehaviour 
{
    public Spawning2 spawner;

    public UnityEvent OnFirstEnter, OnRoomEnter, OnRoomLeave;
    public bool active = false;
    bool unEntered = true;

    public float timeToRegisterInside = .5f;
    float timeInside;

    public float camSize = 7;
    public bool centerCameraOnRoom;
    public Vector3 camOffset;
    public CameraBounds roomXBounds, roomYBounds;

    CameraFollow cam;

    private void Awake()
    {
        cam = CameraFollow.cam;
    }

    private void Start()
    {
        roomXBounds = CameraBounds.ConvertLocalToWorldBounds(roomXBounds, transform.position.x);
        roomYBounds = CameraBounds.ConvertLocalToWorldBounds(roomYBounds, transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        timeInside = 0;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        timeInside += Time.deltaTime;
        if (timeInside >= timeToRegisterInside)
        {
            if (unEntered)
            {
                OnFirstEnter?.Invoke();
                unEntered = false;
            }
                
            ToggleRoom(true);
        }

        if (centerCameraOnRoom)
        {
            cam.cameraSize = camSize;
            cam.cameraCenter = transform;
            cam.lockToCenter = true;
            cam.offset = camOffset;

            cam.ChangeRoomBounds(roomXBounds, roomYBounds);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        ToggleRoom(false);

        cam.ResetCameraSize();
        cam.ResetCameraBounds();
        cam.cameraCenter = null;
        cam.lockToCenter = false;
    }

    public void ToggleRoom(bool val)
    {
        if (!active && val)
        {
            OnRoomEnter?.Invoke();
        } else
        {
            OnRoomLeave?.Invoke();
        }

        active = val;
    }

    public void ToggleSpawns(bool val)
    {
        if (!spawner)
        {
            print("No Spawner Assigned.");
            return;
        }

            if (!val) spawner.CancelInvoke();
        else spawner.StartSpawner();
    }
}
