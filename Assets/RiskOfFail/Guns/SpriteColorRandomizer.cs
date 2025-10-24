using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class SpriteColorRandomizer : MonoBehaviour
{
    [SerializeField] private BrushColor[] colors;
    [SerializeField] private Animator anim;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private Light2D _light;
    [SerializeField] private ParticleSystem ps;

    private Projectile p;

    private void Awake()
    {
        p = GetComponent<Projectile>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        var color = Random.Range(0, colors.Length - 1);
        anim.Play(colors[color].animationName);
        trail.startColor = colors[color].color;
        trail.endColor = colors[color].color;
        p.hitmarkerPrefab = colors[color].hitmarkerPrefab;
        _light.color = colors[color].color;

        var ps = this.ps.main;
        ps.startColor = colors[color].color;
    }

    [Serializable]
    private struct BrushColor
    {
        public Color color;
        public string animationName;
        public GameObject hitmarkerPrefab;
    }
}