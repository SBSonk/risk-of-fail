using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public CameraBounds camBoundX, camBoundY;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void DeactivateRoom(float t)
    {
        anim.Play("RoomFadeout");

        // Deactivate self
        Invoke("Deactivate", t);
    }

    void Deactivate()
    {
        gameObject.SetActive(false);

        // Recalculate AI path
        AstarPath.active.Scan();
    }
}
