using UnityEngine;
using UnityEngine.UI;

public class MoveCursor : MonoBehaviour
{
    Image cursor;
    [SerializeField] float rotationDegrees = 1, lerpPos = 0.5f;
    Vector3 mousePos;

    private void Awake()
    {
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
        return InputManager.mouseRawPosition;
    }

    public void ChangeCrosshair(Crosshair c)
    {
        cursor.sprite = c.sprite;
        rotationDegrees = c.rotationSpeed;
    }
}

[System.Serializable]
public struct Crosshair
{
    public Sprite sprite;
    public float rotationSpeed;
}