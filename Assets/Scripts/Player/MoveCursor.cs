using UnityEngine;
using UnityEngine.UI;

public class MoveCursor : MonoBehaviour
{
    public static MoveCursor main;

    Image cursor;
    [SerializeField] float rotationDegrees = 1, lerpPos = 0.5f;
    Vector3 mousePos;

    [SerializeField] Crosshair menuCrosshair;
    Crosshair lastCrosshair;

    private void Awake()
    {
        main = this;
        cursor = GetComponentInChildren<Image>();
    }

    private void Start()
    {
        // Hide mouse
        Cursor.visible = false;
    }

    void Update()
    {
        mousePos = GetMouseInput();

        transform.SetPositionAndRotation(Vector3.Lerp(transform.position, mousePos, lerpPos), Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + (rotationDegrees * Time.deltaTime)));
    }

    // Returns mouse input for this frame
    Vector3 GetMouseInput()
    {
        return Input.mousePosition;
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
}

