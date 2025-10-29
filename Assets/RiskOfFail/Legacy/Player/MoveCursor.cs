using RiskOfFail.Combat;
using UnityEngine;
using UnityEngine.UI;

public class MoveCursor : MonoBehaviour
{
    public static MoveCursor instance;

    public bool visible = true;

    [Header("Visual Settings")] [SerializeField]
    private float rotationDegrees = 1, lerpPos = 0.5f;

    [SerializeField] private Crosshair menuCrosshair;

    private Camera cam;
    private Image cursor;
    private Crosshair lastCrosshair;
    private Transform player;

    private void Awake()
    {
        instance = this;
        cursor = GetComponentInChildren<Image>();

        cam = Camera.main;
    }

    private void Start()
    {
        player = PlayerStatus.instance.transform;

        // Hide mouse
        Cursor.visible = false;
    }

    private void Update()
    {
        // Clamp mouse to game
        Vector3 targetPosition = GetMousePosition();
        targetPosition.x = Mathf.Clamp(targetPosition.x, 0, Screen.width);
        targetPosition.y = Mathf.Clamp(targetPosition.y, 0, Screen.height);
        
        transform.SetPositionAndRotation(Vector3.Lerp(transform.position, targetPosition, lerpPos),
            Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + rotationDegrees * Time.deltaTime));
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }

    private Vector3 GetMousePosition()
    {
        return Input.mousePosition;
    }

    public Vector3 GetMousePlayerDirection()
    {
        var mouseWorldPos = cam.ScreenToWorldPoint(GetMousePosition());
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

    public bool GetVisibility()
    {
        return visible;
    }
}