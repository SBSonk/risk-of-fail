using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public CameraBounds camBoundX, camBoundY;
    Spawning2 spawns;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void DeactivateRoom(float t)
    {
        // Deactivate spawning
        Invoke("Deactivate", t);
    }

    void Deactivate()
    {
        spawns.enabled = false;
    }
}
