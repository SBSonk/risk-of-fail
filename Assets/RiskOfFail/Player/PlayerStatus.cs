using System.Collections;
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

        public float defaultSpeed = 10;
        public ParticleSystem deathParticles;

        public string normalLayer, immuneLayer;
        public static bool IsAlive { get; private set; }

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
            //CameraFollow.cam.SetPlayer(GetComponent<Rigidbody2D>());
        }

        private void Update()
        {
            gameObject.layer = immune ? LayerMask.NameToLayer(immuneLayer) : LayerMask.NameToLayer(normalLayer);
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

        private IEnumerator TakeStun(float duration)
        {
            pMovement.moveSpeed = defaultSpeed / 2;
            stunned = true;

            // Disable switching animations
            pAnimations.canSwitchAnimation = false;

            yield return new WaitForSeconds(duration);

            pMovement.moveSpeed = defaultSpeed;
            stunned = false;

            // Reenable animation switching
            pAnimations.canSwitchAnimation = true;
        }
    }
}