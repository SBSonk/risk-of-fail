using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class Room : MonoBehaviour
{
    public Spawning2 spawner;
    public Light2D light;
    public float openLightIntensity = 0.8f, closeLightIntensity = 0.2f, lightSwitchTime = 0.2f;
    public bool openLightsPermanently = true;

    public UnityEvent OnFirstEnter, OnRoomEnter, OnRoomLeave;
    public bool active;

    public float timeToRegisterInside;
    private float timeInside;
    private bool unEntered = true;

    private void Awake()
    {
        if (!spawner)
        {
            print("Assigning to local spawner.");
            spawner = GetComponent<Spawning2>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        timeInside = 0;

        StopCoroutine("LeaveTimer");
        if (light) StartCoroutine(OpenLights());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (light && !openLightsPermanently) StartCoroutine(CloseLights());
        StartCoroutine(LeaveTimer());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        timeInside += Time.deltaTime;
        if (timeInside >= timeToRegisterInside)
        {
            if (unEntered)
            {
                OnFirstEnter?.Invoke();
                unEntered = false;
            }

            ToggleRoom(true);
        }
    }

    private IEnumerator LeaveTimer()
    {
        yield return new WaitForSeconds(timeToRegisterInside);

        ToggleRoom(false);
    }

    public void ToggleRoom(bool val)
    {
        if (!active && val)
            OnRoomEnter?.Invoke();
        else if (active && !val) OnRoomLeave?.Invoke();

        active = val;
    }

    private IEnumerator OpenLights()
    {
        float t = 0;
        var startIntensity = light.intensity;
        while (t < lightSwitchTime)
        {
            light.intensity = Mathf.Lerp(startIntensity, openLightIntensity, t / lightSwitchTime);

            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
        }

        light.intensity = openLightIntensity;
    }

    private IEnumerator CloseLights()
    {
        float t = 0;
        var startIntensity = light.intensity;
        while (t < lightSwitchTime)
        {
            light.intensity = Mathf.Lerp(startIntensity, closeLightIntensity, t / lightSwitchTime);

            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
        }

        light.intensity = closeLightIntensity;
    }

    public void ToggleSpawns(bool val)
    {
        if (!spawner)
        {
            print("No Spawner Assigned.");
            return;
        }

        if (!val) spawner.CancelInvoke();
        else spawner.StartSpawner();
    }

    public void SetOpenLightIntensity(float intensity)
    {
        openLightIntensity = intensity;
    }

    public void SetCloseLightIntensity(float intensity)
    {
        closeLightIntensity = intensity;
    }

    public void SetOpenLightsPermanently(bool val)
    {
        openLightsPermanently = val;
    }

    public void OpenLightsExt()
    {
        StartCoroutine(OpenLights());
        // Bad function names
    }

    public void CloseLightsExt()
    {
        StartCoroutine(CloseLights());
    }
}