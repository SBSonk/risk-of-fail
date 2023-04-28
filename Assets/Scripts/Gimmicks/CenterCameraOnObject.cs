using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class CenterCameraOnObject : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera objectCamera;
    [SerializeField] float holdTime = 1;
    
    public UnityEvent OnCenterStart, OnCentered, OnRetract;

    public void CenterCamera()
    {
        StopAllCoroutines();
        StartCoroutine(CenterAnimation());
    }

    IEnumerator CenterAnimation()
    {
        // switch to camera
        OnCenterStart?.Invoke();
        objectCamera.enabled = true;

        Time.timeScale = 0;

        // wait to arrive destination
        yield return new WaitForSecondsRealtime(1.5f);
        OnCentered?.Invoke();
        
        yield return new WaitForSecondsRealtime(holdTime);
        
        
        // return to camera
        objectCamera.enabled = false;

        yield return new WaitForSecondsRealtime(1.5f);
        
        OnRetract?.Invoke();
        Time.timeScale = 1;
    }
}
