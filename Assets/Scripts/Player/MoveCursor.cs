using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCursor : MonoBehaviour
{
    SpriteRenderer cursor;
    [SerializeField] float rotationDegrees = 1, lerpPos = 0.5f;
    Vector3 mousePos;

    private void Awake()
    {
        cursor = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        // Hide mouse
        Cursor.visible = false;
    }

    void Update()
    {
        mousePos = GetMouseInput();

        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + (rotationDegrees * Time.deltaTime));
        transform.position = Vector3.Lerp(transform.position, mousePos, lerpPos);
    }

    // Returns mouse input for this frame
    Vector3 GetMouseInput()
    {
        return InputManager.mousePosition;
    }

    public void ChangeCrosshair(Sprite sprite)
    {
        cursor.sprite = sprite;
    }
}