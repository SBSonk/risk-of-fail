using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Key : MonoBehaviour
{
    [SerializeField] Door[] doorsToOpen;
    [SerializeField] private ParticleSystem openParticles;
    [SerializeField] private SpriteRenderer sprite;

    const float OPENANIMATIONTIME = 0.75f;
    
    public void TryOpenDoor(Door door)
    {
        foreach (var d in doorsToOpen)
        {
            if (d != door) continue;
            
            // Start animation
            StartCoroutine(OpenDoorAnimation(d));
            return;
        }
    }

    IEnumerator OpenDoorAnimation(Door d)
    {
        transform.parent.GetComponent<FollowBehindPlayer>().enabled = false;
        sprite.sortingLayerName = "Ceiling";

        float t = 0;
        Vector3 startPos = transform.parent.position;
        while (t < OPENANIMATIONTIME)
        {
            transform.parent.position = Vector3.Slerp(startPos, d.transform.position, AnimationCurve.EaseInOut(0, 0, OPENANIMATIONTIME, 1).Evaluate(t/OPENANIMATIONTIME));
            t += Time.deltaTime;
            yield return null;
        }

        transform.parent.position = d.transform.position;
        d.ToggleDoor(true);
        openParticles.Play();
        sprite.enabled = false;
        Destroy(transform.parent.gameObject, 1);
    }
}
