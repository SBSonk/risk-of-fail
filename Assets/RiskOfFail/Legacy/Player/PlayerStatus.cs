using System.Collections;
using RiskOfFail.Combat.Enums;
using UnityEngine;

namespace RiskOfFail.Combat
{
    public class PlayerStatus : Alive
    {
        public static PlayerStatus player;

        public PlayerMovement pMovement;
        public PlayerShooting pShooting;
        public PlayerAnimations pAnimations;
        public PlayerSFXManager pSFXManager;
        
        public static bool IsAlive { get; private set; }

        private void Awake()
        {
            player = this;
            IsAlive = true;

            pMovement = GetComponent<PlayerMovement>();
            pShooting = GetComponent<PlayerShooting>();
            pAnimations = GetComponent<PlayerAnimations>();
            pSFXManager = GetComponent<PlayerSFXManager>();
        }

        private void Start()
        {
            pShooting.Initialize();
            pAnimations.Initialize(pShooting, pMovement, this);
            pSFXManager.Initialize(pShooting, pMovement);

            ObjectFade.player = transform;
            HudManager4.hud.SetPlayer(this);
            //CameraFollow.cam.SetPlayer(GetComponent<Rigidbody2D>());
        }

        protected override void Death(DamageTypeFlag flag)
        {
            dead = true;
            IsAlive = false;

            pMovement.enabled = false;
            pShooting.enabled = false;
            pAnimations.enabled = false;

            onDeath?.Invoke(flag);
        }
    }
}