using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteColorRandomizer : MonoBehaviour
{
    [SerializeField] Color[] colors;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] TrailRenderer trail;
    [SerializeField] ParticleSystem hitmarker;
    [SerializeField] Light light;

    private void Start()
    {
        int color = Random.Range(0, colors.Length - 1);
        sprite.color = colors[color];
        trail.startColor = colors[color];
        hitmarker.startColor = colors[color]; // Deprecated but I can't be bothered reading the docs to find the new way
        light.color = colors[color];
    }
}
