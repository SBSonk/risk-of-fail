using System;
using System.Collections;
using GameAudioScriptingEssentials;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatus : Alive
{
    public static PlayerStatus player;
    public static bool IsAlive { get; private set; }

    public PlayerMovement pMovement;
    public PlayerShooting pShooting;
    public PlayerAnimations pAnimations;
    public PlayerSFXManager pSFXManager;

    public float defaultSpeed = 10;
    public ParticleSystem deathParticles;

    private void Awake()
    {
        player = this;
        IsAlive = true;

        pMovement = GetComponent<PlayerMovement>();
        pShooting = GetComponent<PlayerShooting>();
        pAnimations = GetComponent<PlayerAnimations>();
        pSFXManager = GetComponent<PlayerSFXManager>();

        pMovement.moveSpeed = defaultSpeed;
        pMovement.baseSpeed = defaultSpeed;
    }

    private void Start()
    {
        pShooting.Initialize();
        pAnimations.Initialize(pShooting, pMovement, this);
        pSFXManager.Initialize(pShooting, pMovement);

        ObjectFade.player = transform;
        HudManager4.hud.SetPlayer(this);
        CameraFollow.cam.SetPlayer(GetComponent<Rigidbody2D>());
    }
    
    protected override void Death(KillFlag flag)
    {
        dead = true;
        IsAlive = false;

        pMovement.enabled = false;
        pShooting.enabled = false;
        pAnimations.enabled = false;

        onDeath?.Invoke(flag);
        deathParticles.transform.parent = null;
        deathParticles.Play();

        Instantiate(deathSound);
    }
    
    public override void Stun(float duration)
    {
        StartCoroutine(TakeStun(duration));
    }

    IEnumerator TakeStun(float duration)
    {
        pMovement.moveSpeed = defaultSpeed / 2;

        // Disable switching animations
        pAnimations.canSwitchAnimation = false;

        yield return new WaitForSeconds(duration);

        pMovement.moveSpeed = defaultSpeed;

        // Reenable animation switching
        pAnimations.canSwitchAnimation = true;

    }
}
