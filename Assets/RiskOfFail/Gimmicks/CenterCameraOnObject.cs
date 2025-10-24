using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class CenterCameraOnObject : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera objectCamera;
    [SerializeField] private float holdTime = 1;
    [SerializeField] private bool unPauseOnCenter;

    public UnityEvent OnCenterStart, OnCentered, OnRetract;

    public void CenterCamera()
    {
        StopAllCoroutines();
        StartCoroutine(CenterAnimation());
    }

    private IEnumerator CenterAnimation()
    {
        // switch to camera
        OnCenterStart?.Invoke();
        objectCamera.enabled = true;

        Time.timeScale = 0;

        // wait to arrive destination
        yield return new WaitForSecondsRealtime(1.5f);
        OnCentered?.Invoke();

        if (unPauseOnCenter) Time.timeScale = 1;

        yield return new WaitForSecondsRealtime(holdTime);


        // return to camera
        objectCamera.enabled = false;

        yield return new WaitForSecondsRealtime(1.5f);

        OnRetract?.Invoke();
        Time.timeScale = 1;
    }
}