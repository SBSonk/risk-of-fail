using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow cam;

    [SerializeField] public Vector3 offset;
    [SerializeField] float lerpVal = 0.15f, roomViewLerp = 0.05f, maxCameraPredict = .5f;

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

    bool cameraControl = true;

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
        if (!PlayerStatus.IsAlive) return;
        if (!cameraControl) return;

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
        desiredPos += offset + Vector3.back;

        /*if (player.velocity.magnitude > 0) lastPlayerVel = Vector3.Lerp(lastPlayerVel, player.velocity.normalized, lValue);*/
        float xPlayerVel, yPlayerVel;
        xPlayerVel = InputManager.playerDirection.x != 0 ? player.velocity.x : lastPlayerVel.x;
        yPlayerVel = InputManager.playerDirection.y != 0 ? player.velocity.y : lastPlayerVel.y;

        lastPlayerVel = Vector3.Lerp(lastPlayerVel, new Vector3(xPlayerVel, yPlayerVel), 0.005f);
        lastPlayerVel.x = Mathf.Clamp(lastPlayerVel.x, -maxCameraPredict, maxCameraPredict);
        lastPlayerVel.y = Mathf.Clamp(lastPlayerVel.y, -maxCameraPredict, maxCameraPredict);


        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, cameraSize, lerpVal);
        transform.position = Vector3.Lerp(transform.position, desiredPos, lValue);
    }

    public void ChangeRoomBounds(CameraBounds x, CameraBounds y)
    {
        worldBoundsX = x;
        worldBoundsY = y;
    }

    public IEnumerator CenterCameraOnPosition(CenterCameraOnObject c, Vector3 position, float time, float holdTime, Vector3 offset, bool freezeGame)
    {
        c.OnCenterStart?.Invoke();
        if (freezeGame) Time.timeScale = 0;

        cameraControl = false;
        Vector3 startPos = transform.position;

        float t = 0;
        while (t <= time)
        {
            if (freezeGame)
            {
                yield return new WaitForSecondsRealtime(0.01f);
                t += 0.01f;
            }
            else
            {
                yield return new WaitForEndOfFrame();
                t += Time.deltaTime;
            }
            
            transform.position = Vector3.Lerp(startPos, position + offset, t / time);
        }

        c.OnCentered?.Invoke();
        if (freezeGame) yield return new WaitForSecondsRealtime(holdTime);
        else yield return new WaitForSeconds(holdTime);

        cameraControl = true;
        if (freezeGame) Time.timeScale = 1;
        c.OnRetract?.Invoke();
    }

    public IEnumerator CenterCameraOnPosition(CenterCameraOnObject c, Vector3[] positions, float time, float holdTime, Vector3 offset, bool freezeGame = false)
    {
        c.OnCenterStart?.Invoke();
        if (freezeGame) Time.timeScale = 0;

        cameraControl = false;
        Vector3 startPos = transform.position;

        Vector3 position = Vector3.zero;
        // Get average of positions
        int i;
        for (i = 0; i < positions.Length; i++)
        {
            position += positions[i];
        }
        position /= i;

        float t = 0;
        while (t <= time)
        {
            if (freezeGame)
            {
                yield return new WaitForSecondsRealtime(0.01f);
                t += 0.01f;
            }
            else
            {
                yield return new WaitForEndOfFrame();
                t += Time.deltaTime;
            }

            transform.position = Vector3.Lerp(startPos, position + offset, t / time);
        }

        c.OnCentered?.Invoke();
        if (freezeGame) yield return new WaitForSecondsRealtime(holdTime);
        else yield return new WaitForSeconds(holdTime);

        cameraControl = true;
        if (freezeGame) Time.timeScale = 1;
        c.OnRetract?.Invoke();
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