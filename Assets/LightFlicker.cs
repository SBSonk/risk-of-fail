using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    public float defaultIntensity = 0.25f;
    public float maxTimeBetweenFlickers = 1f;
    public Light2D _light;

    private void Start()
    {
        StartCoroutine(Flicker(maxTimeBetweenFlickers));
    }

    IEnumerator Flicker(float t)
    {
        // theres somewhere to copy paste this
        yield break;
    }
}
