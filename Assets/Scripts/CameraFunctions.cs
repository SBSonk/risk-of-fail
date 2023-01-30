using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFunctions : MonoBehaviour
{
    public static CameraFunctions main;

    [SerializeField] Vector3 defaultOffset;
    Vector3 offset;
    [SerializeField] float lerp = 0.5f;

    private void Awake()
    {
        if (!main) main = this;
    }

    private void FixedUpdate()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, offset + new Vector3(0f, 0f, -10f), lerp);
    }

    [ContextMenu("ScreenShake")]
    public void DoScreenShake(ScreenshakeValue shake)
    {
        StartCoroutine(ScreenShake(shake));
    }

    IEnumerator ScreenShake(ScreenshakeValue shake)
    {
        float finalTime = Time.time + shake.duration;

        while (Time.time < finalTime)
        {
            var noise = Mathf.PerlinNoise(Time.time, Time.time);
            offset = new Vector2(shake.magnitude * Random.Range(-1f, 1f), shake.magnitude * Random.Range(-1f, 1f)) * noise;
            yield return new WaitForEndOfFrame();
        }

        offset = defaultOffset;
    }
}

[System.Serializable]
public struct ScreenshakeValue
{
    public float duration, magnitude;
}