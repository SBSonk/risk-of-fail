using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CenterCameraOnObject : MonoBehaviour
{
    [SerializeField] Transform[] transforms;
    [SerializeField] float time = 1, holdTime = 1;
    [SerializeField] bool freezeGame = false;

    CameraFollow cam;

    public UnityEvent OnCenterStart, OnCentered, OnRetract;

    private void Start()
    {
        cam = CameraFollow.cam;
    }

    public void CenterCamera()
    {
        if (transforms.Length == 1)
        {
            cam.StartCoroutine(cam.CenterCameraOnPosition(this, transforms[0].position, time, holdTime, freezeGame));
        }
        else
        {
            Vector3[] positions = new Vector3[transforms.Length];
            for (int i = 0; i < transforms.Length; i++)
            {
                positions[i] = transforms[i].position;
            }

            cam.StartCoroutine(cam.CenterCameraOnMultiplePositions(this, positions, time, holdTime, freezeGame));
        }
    }
}
