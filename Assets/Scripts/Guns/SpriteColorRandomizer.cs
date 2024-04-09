using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class SpriteColorRandomizer : MonoBehaviour
{
    [SerializeField] BrushColor[] colors;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] TrailRenderer trail;
    [SerializeField] ParticleSystem hitmarker;
    [SerializeField] Light2D _light;

    private Projectile p;

    private void Awake()
    {
        p = GetComponent<Projectile>();
    }

    private void Start()
    {
        int color = Random.Range(0, colors.Length - 1);
        sprite.color = colors[color].color;
        trail.startColor = colors[color].color;
        p.hitmarkerPrefab = colors[color].hitmarkerPrefab; 
        _light.color = colors[color].color;
    }
    
    [System.Serializable]
    struct BrushColor
    {
        public Color color;
        public GameObject hitmarkerPrefab;
    }
}
