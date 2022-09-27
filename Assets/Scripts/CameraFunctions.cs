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
        StartCoroutine(ScreenShake(shake.duration, Vector2.one * shake.magnitude, shake.shakespeed));
    }

    IEnumerator ScreenShake(float duration, Vector2 magnitude, float shakeSpeed)
    {
        float finalTime = Time.time + duration;

        while (Time.time < finalTime)
        {
            var noise = Mathf.PerlinNoise(Time.time, Time.time);
            offset = new Vector2(magnitude.x * Random.Range(-1f, 1f), magnitude.y * Random.Range(-1f, 1f)) * noise;
            yield return new WaitForSeconds(shakeSpeed);
        }

        offset = defaultOffset;
    }
}

[System.Serializable]
public struct ScreenshakeValue
{
    public float duration, magnitude, shakespeed;
}