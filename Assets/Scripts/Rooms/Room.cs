using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Room : MonoBehaviour
{
    [SerializeField] GameObject collision;
    public Spawning2 spawner;

    public UnityEvent OnFirstEnter, OnRoomEnter, OnRoomLeave;
    public bool active = false;
    bool unEntered = true;

    public float timeToRegisterInside = 1f;
    float timeInside;

    public float camSize = 7;
    public bool centerCameraOnRoom;
    public Vector3 camOffset;
    
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
            var c = CameraFollow.cam;
            c.cameraSize = camSize;
            c.cameraCenter = transform.position;
            c.lockToCenter = true;
            c.offset = camOffset;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        ToggleRoom(false);

        CameraFollow.cam.ResetCameraSize();
        CameraFollow.cam.cameraCenter = Vector3.zero;
        CameraFollow.cam.lockToCenter = false;
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
        if (!spawner) return;

        spawner.enabled = val;

        if (!val) spawner.CancelInvoke();
        else spawner.StartSpawner();
    }
}
