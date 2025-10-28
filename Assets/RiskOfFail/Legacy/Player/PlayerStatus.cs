using System;
using System.Collections;
using NaughtyAttributes;
using RiskOfFail.Combat.Enums;
using UnityEngine;

namespace RiskOfFail.Combat
{
    public class PlayerStatus : Alive
    {
        public static PlayerStatus instance;

        public PlayerMovement movement { private set; get; }
        public PlayerShooting shooting { private set; get; }
        public PlayerAnimations animations { private set; get; }
        public PlayerSFXManager sfx { private set; get; }

        private void Start()
        {
            instance = this;
        }

        private void OnDestroy()
        {
            instance = null;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void Death(DamageTypeFlag flag)
        {
            movement.enabled = false;
            shooting.enabled = false;
            animations.enabled = false;
            sfx.enabled = false;
            
            base.Death(flag);
        }
        
        #if UNITY_EDITOR

        [Button("Do 10 Damage")]
        void DEBUG_Do10Damage() => GiveDamage(10f, .5f, DamageTypeFlag.Self);

        #endif
    }
}