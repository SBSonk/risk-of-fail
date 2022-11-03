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

    IEnumerator Flicker(float max)
    {
        float t = 0.1f;
        while (true)
        {
            print("test");
            t = Random.Range(0.1f, max);
            float time = 0;
            while (time < t)
            {
                _light.intensity = Mathf.Lerp(0, defaultIntensity, time / t);
                time += Time.deltaTime;
            }
            // CRASHES double while loop lol
        } // this looks bad even when it was working so ill pretend this doesnt exist
    }
}
