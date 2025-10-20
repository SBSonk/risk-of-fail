using System;
using UnityEngine;
using UnityEngine.UI;

public class MoveCursor : MonoBehaviour
{
    public static MoveCursor instance;

    public bool visible = true;
    
    [Header("Visual Settings")]
    [SerializeField] float rotationDegrees = 1, lerpPos = 0.5f;

    [SerializeField] Crosshair menuCrosshair;
    Crosshair lastCrosshair;

    private Camera cam;
    private Transform player;
    Image cursor;

    private void Awake()
    {
        instance = this;
        cursor = GetComponentInChildren<Image>();
        
        cam = Camera.main;
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }

    private void Start()
    {
        player = PlayerStatus.player.transform;
        
        // Hide mouse
        Cursor.visible = false;
    }

    void Update()
    {
        transform.SetPositionAndRotation(Vector3.Lerp(transform.position, GetMousePosition(), lerpPos), Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + (rotationDegrees * Time.deltaTime)));
    }

    Vector3 GetMousePosition()
    {
        return Input.mousePosition;
    }

    public Vector3 GetMousePlayerDirection()
    {
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(GetMousePosition());
        mouseWorldPos.z = 0;
        return (mouseWorldPos - player.position).normalized;
    }

    public void ChangeCrosshair(Crosshair c)
    {
        cursor.sprite = c.sprite;
        rotationDegrees = c.rotationSpeed;

        lastCrosshair = c;
    }

    public void EnterMenu()
    {
        cursor.sprite = menuCrosshair.sprite;
        rotationDegrees = menuCrosshair.rotationSpeed;
    }

    public void LeaveMenu()
    {
        ChangeCrosshair(lastCrosshair);
    }

    public void SetVisibility(bool val)
    {
        visible = val;
        
        cursor.enabled = visible;
    }

    public bool GetVisibility() => visible;
}

