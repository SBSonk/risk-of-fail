using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BossIntroCutscene : MonoBehaviour
{
    public Light2D globalLight;
    public GameObject phaseOne, table;

    public void StartFight()
    {
        phaseOne.SetActive(true);
        gameObject.SetActive(false);
        table.SetActive(false);
    }

    private IEnumerator TurnOnLights()
    {
        var timeToLight = .1f;
        float time = 0;
        while (time < timeToLight)
        {
            globalLight.intensity = Mathf.Lerp(.4f, 1, time / timeToLight);

            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        globalLight.intensity = 1;
    }
}