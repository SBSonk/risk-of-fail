using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpriteColorRandomizer : MonoBehaviour
{
    [SerializeField] Color[] colors;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] TrailRenderer trail;
    [SerializeField] ParticleSystem hitmarker;
    [SerializeField] Light2D _light;

    private void Start()
    {
        int color = Random.Range(0, colors.Length - 1);
        sprite.color = colors[color];
        trail.startColor = colors[color];
        hitmarker.startColor = colors[color]; // Deprecated but I can't be bothered reading the docs to find the new way
        _light.color = colors[color];
    }
}
