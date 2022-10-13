using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionActiveRoom : MonoBehaviour
{
    public CameraFollow _camera;
    [SerializeField] float timeToDisappear = 0.5f;
    [SerializeField] Room lastRoom, newRoom;

    private void Awake()
    {
        _camera = GameObject.Find("CameraPivot").GetComponent<CameraFollow>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        newRoom.gameObject.SetActive(true);
        if (lastRoom) DisableLastRoom();

        // Convert local room bound to global
        CameraBounds x = ConvertLocalToGlobalBounds(newRoom.camBoundX, newRoom.transform.position.x);
        CameraBounds y = ConvertLocalToGlobalBounds(newRoom.camBoundY, newRoom.transform.position.y);

        // Change room bounds
        _camera.ChangeRoomBounds(x, y);
    }

    void DisableLastRoom() {
        lastRoom.DeactivateRoom(timeToDisappear);
    }

    CameraBounds ConvertLocalToGlobalBounds(CameraBounds bounds, float axisPos)
    {
        bounds.max += axisPos;
        bounds.min += axisPos;

        return bounds;
    }
}
