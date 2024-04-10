using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class SpriteColorRandomizer : MonoBehaviour
{
    [SerializeField] BrushColor[] colors;
    [SerializeField] Animator anim;
    [SerializeField] TrailRenderer trail;
    [SerializeField] Light2D _light;
    [SerializeField] private ParticleSystem ps;
    
    private Projectile p;

    private void Awake()
    {
        p = GetComponent<Projectile>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        int color = Random.Range(0, colors.Length - 1);
        anim.Play(colors[color].animationName);
        trail.startColor = colors[color].color;
        trail.endColor = colors[color].color;
        p.hitmarkerPrefab = colors[color].hitmarkerPrefab; 
        _light.color = colors[color].color;

        var ps = this.ps.main;
        ps.startColor = colors[color].color;
    }
    
    [System.Serializable]
    struct BrushColor
    {
        public Color color;
        public string animationName;
        public GameObject hitmarkerPrefab;
    }
}
