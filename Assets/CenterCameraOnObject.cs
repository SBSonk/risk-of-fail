using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCameraOnObject : MonoBehaviour
{
    [SerializeField] Transform[] transforms;
    [SerializeField] float time = 1, holdTime = 1;
    [SerializeField] bool freezeGame = false;

    CameraFollow cam;

    private void Start()
    {
        cam = CameraFollow.cam;
    }

    public void CenterCamera()
    {
        if (transforms.Length == 1)
        {
            cam.StartCoroutine(cam.CenterCameraOnPosition(transforms[0].position, time, holdTime, freezeGame));
        }
        else
        {
            Vector3[] positions = new Vector3[transforms.Length];
            for (int i = 0; i < transforms.Length; i++)
            {
                positions[i] = transforms[i].position;
            }

            cam.StartCoroutine(cam.CenterCameraOnMultiplePositions(positions, time, holdTime, freezeGame));
        }
    }
}
